using Microsoft.EntityFrameworkCore;

public class ShipmentAccess : BaseAccess<Shipment>
{
    public ShipmentAccess(ApplicationDbContext context) : base(context) { }

    public override async Task<List<Shipment>> GetAll()
    {
        List<Shipment> shipments = await _context.Set<Shipment>().AsNoTracking()
            .Include(shipment => shipment.Items)
            .ToListAsync();
        return shipments;
    }

    public override async Task<Shipment?> GetById(int shipmentId)
    {
        Shipment? shipment = await _context.Set<Shipment>().AsNoTracking()
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == shipmentId);
        return shipment;
    }

    public override async Task<bool> Update(Shipment shipment)
    {
        if (shipment == null) return false;

        DetachEntity(shipment);

        var existingOrder = await GetById(shipment.Id!);
        if (existingOrder == null) return false;

        if (existingOrder.Items != null)
        {
            foreach (ShipmentItemMovement item in existingOrder.Items)
            {
                var existingItem = await _context.Set<ShipmentItemMovement>().FirstOrDefaultAsync(i => i.Id == item.Id);
                if (existingItem != null)
                {
                    existingItem.Amount = item.Amount;
                    existingItem.ItemId = item.ItemId;
                }
                else _context.Set<ShipmentItemMovement>().Add(item);
            }
        }
        _context.Update(shipment);
        var changes = await _context.SaveChangesAsync();
        ClearChangeTracker();
        return changes > 0;
    }
}