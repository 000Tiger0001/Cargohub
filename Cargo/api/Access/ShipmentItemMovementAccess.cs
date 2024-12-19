using Microsoft.EntityFrameworkCore;

public class ShipmentItemMovementAccess : BaseAccess<ShipmentItemMovement>
{
    public ShipmentItemMovementAccess(ApplicationDbContext context) : base(context) { }
    public async Task<List<ShipmentItemMovement>> GetAllByOrderId(int shipmentId)
    {
        return await DB.AsNoTracking().Where(entity => entity.Shipment_Id == shipmentId).ToListAsync();
    }
}