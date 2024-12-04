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
}