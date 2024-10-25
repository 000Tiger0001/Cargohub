public class Orders
{
    public Guid Id { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string Reference { get; set; }
    public string ExtraReference { get; set; }
    public string OrderStatus { get; set; }
    public string Notes { get; set; }
    public string ShippingNotes { get; set; }
    public string PickingNotes { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid ShipTo { get; set; }
    public Guid BillTo { get; set; }
    public Guid ShipmentId { get; set; }
    public double TotalAmount { get; set; }
    public double Totaldiscount { get; set; }
    public double TotalTax { get; set; }
    public double TotalSurcharge { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Items> Items { get; set; }

    public Orders(int sourceId, DateTime orderDate, DateTime requestdate, string reference, string extraReference, string orderStatus, string notes, string shippingNotes, string pickingNotes, Guid warehouseId, Guid shipTo, Guid billTo, Guid shipmentId, double totalAmount, double totalDiscount, double totalTax, double totalSurcharge, DateTime createdAt, DateTime updatedAt, List<Items> items)
    {
        Id = Guid.NewGuid();
        SourceId = sourceId;
        OrderDate = orderDate;
        RequestDate = requestdate;
        Reference = reference;
        ExtraReference = extraReference;
        OrderStatus = orderStatus;
        Notes = notes;
        ShippingNotes = shippingNotes;
        PickingNotes = pickingNotes;
        WarehouseId = warehouseId;
        ShipTo = shipTo;
        BillTo = billTo;
        ShipmentId = shipmentId;
        TotalAmount = totalAmount;
        Totaldiscount = totalDiscount;
        TotalTax = totalTax;
        TotalSurcharge = totalSurcharge;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Items = items;
    }
}