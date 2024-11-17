using Newtonsoft.Json;

public class Order : IHasId
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("source_id")]
    public string? SourceId { get; set; }

    [JsonProperty("order_date")]
    public DateTime OrderDate { get; set; }

    [JsonProperty("request_date")]
    public DateTime RequestDate { get; set; }

    [JsonProperty("reference")]
    public string? Reference { get; set; }

    [JsonProperty("reference_extra")]
    public string? ExtraReference { get; set; }

    [JsonProperty("order_status")]
    public string? OrderStatus { get; set; }

    [JsonProperty("notes")]
    public string? Notes { get; set; }

    [JsonProperty("shipping_notes")]
    public string? ShippingNotes { get; set; }

    [JsonProperty("picking_notes")]
    public string? PickingNotes { get; set; }

    [JsonProperty("warehouse_id")]
    public int WarehouseId { get; set; }

    [JsonProperty("ship_to")]
    public string? ShipTo { get; set; }

    [JsonProperty("bill_to")]
    public string? BillTo { get; set; }

    [JsonProperty("shipment_id")]
    public string? ShipmentId { get; set; }

    [JsonProperty("total_amount")]
    public double TotalAmount { get; set; }

    [JsonProperty("total_discount")]
    public double Totaldiscount { get; set; }

    [JsonProperty("total_tax")]
    public double TotalTax { get; set; }

    [JsonProperty("total_surcharge")]
    public double TotalSurcharge { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [JsonProperty("items")]
    public List<ItemMovement>? Items { get; set; }

    public Order() { }

    public Order(string id, string sourceId, DateTime orderDate, DateTime requestdate, string reference, string extraReference, string orderStatus, string notes, string shippingNotes, string pickingNotes, int warehouseId, string shipTo, string billTo, string shipmentId, double totalAmount, double totalDiscount, double totalTax, double totalSurcharge, List<ItemMovement> items)
    {
        Id = id;
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
        Items = items;
    }
}