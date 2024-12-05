public class ShipmentAccess : BaseAccess<Shipment>
{
    private readonly ItemMovementAccess<Shipment, ShipmentItemMovement> _itemMovementAccess;

    public ShipmentAccess(ApplicationDbContext context) : base(context)
    {
        _itemMovementAccess = new ItemMovementAccess<Shipment, ShipmentItemMovement>(context);
    }

    public override async Task<List<Shipment>> GetAll()
    {
        return await _itemMovementAccess.GetAll();
    }

    public override async Task<Shipment?> GetById(int id)
    {
        return await _itemMovementAccess.GetById(id);
    }

    public override async Task<bool> Update(Shipment shipment)
    {
        return await _itemMovementAccess.Update(shipment);
    }
}