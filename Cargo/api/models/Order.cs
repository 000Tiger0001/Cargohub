using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("source_id")]
    public int SourceId { get; set; }

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

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("WarehouseId")]
    public virtual Warehouse? Warehouse { get; set; }

    [JsonProperty("ship_to")]
    public int? ShipTo { get; set; } = null;

    [JsonProperty("bill_to")]
    public int? BillTo { get; set; } = null;

    [JsonProperty("shipment_id")]
    public int? ShipmentId { get; set; } = null;

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("ShipmentId")]
    public virtual Shipment? Shipment { get; set; }

    [JsonProperty("total_amount")]
    public double TotalAmount { get; set; }

    [JsonProperty("total_discount")]
    public double TotalDiscount { get; set; }

    [JsonProperty("total_tax")]
    public double TotalTax { get; set; }

    [JsonProperty("total_surcharge")]
    public double TotalSurcharge { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [JsonProperty("items")]
    public List<OrderItemMovement>? Items { get; set; }

    public Order() { }

    public Order(int id, int sourceId, DateTime orderDate, DateTime requestdate, string reference, string extraReference, string orderStatus, string notes, string shippingNotes, string pickingNotes, int warehouseId, int shipTo, int billTo, int shipmentId, double totalAmount, double totalDiscount, double totalTax, double totalSurcharge, List<OrderItemMovement> items)
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
        TotalDiscount = totalDiscount;
        TotalTax = totalTax;
        TotalSurcharge = totalSurcharge;
        Items = items;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Order order)
        {
            // Sort the items lists before comparing them
            var sortedItems = Items?.OrderBy(i => i.ItemId).ThenBy(i => i.Amount).ToList();
            var sortedOrderItems = order.Items?.OrderBy(i => i.ItemId).ThenBy(i => i.Amount).ToList();

            bool itemsAreTheSame = sortedItems != null && sortedOrderItems != null &&
                       sortedItems.Count == sortedOrderItems.Count &&
                       sortedItems.SequenceEqual(sortedOrderItems);

            return order.Id == Id &&
                   order.SourceId == SourceId &&
                   order.OrderDate == OrderDate &&
                   order.RequestDate == RequestDate &&
                   order.Reference == Reference &&
                   order.ExtraReference == ExtraReference &&
                   order.OrderStatus == OrderStatus &&
                   order.Notes == Notes &&
                   order.ShippingNotes == ShippingNotes &&
                   order.PickingNotes == PickingNotes &&
                   order.WarehouseId == WarehouseId &&
                   order.ShipTo == ShipTo &&
                   order.BillTo == BillTo &&
                   order.ShipmentId == ShipmentId &&
                   order.TotalAmount == TotalAmount &&
                   order.TotalDiscount == TotalDiscount &&
                   order.TotalTax == TotalTax &&
                   order.TotalSurcharge == TotalSurcharge &&
                   itemsAreTheSame;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}