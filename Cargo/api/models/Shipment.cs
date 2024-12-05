using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Shipment : IHasId, IHasItemMovements<ShipmentItemMovement>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("order_id")]
    // leave single order for now, since there is no logic yet to make a shipment for multiple orders
    public int OrderId { get; set; }

    [JsonProperty("source_id")]
    public int SourceId { get; set; }

    [JsonProperty("order_date")]
    public DateTime OrderDate { get; set; }

    [JsonProperty("request_date")]
    public DateTime RequestDate { get; set; }

    [JsonProperty("shipment_date")]
    public DateTime ShipmentDate { get; set; }

    [JsonProperty("shipment_type")]
    public char ShipmentType { get; set; }

    [JsonProperty("shipment_status")]
    public string? ShipmentStatus { get; set; }

    [JsonProperty("notes")]
    public string? Notes { get; set; }

    [JsonProperty("carrier_code")]
    public string? CarrierCode { get; set; }

    [JsonProperty("carrier_description")]
    public string? CarrierDescription { get; set; }

    [JsonProperty("service_code")]
    public string? ServiceCode { get; set; }

    [JsonProperty("payment_type")]
    public string? PaymentType { get; set; }

    [JsonProperty("transfer_mode")]
    public string? TransferMode { get; set; }

    [JsonProperty("total_package_count")]
    public int TotalPackageCount { get; set; }

    [JsonProperty("total_package_weight")]
    public double TotalPackageWeight { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [JsonProperty("items")]
    public List<ShipmentItemMovement>? Items { get; set; }

    public Shipment() { }

    public Shipment(int id, int orderId, int sourceId, DateTime orderDate, DateTime requestDate, DateTime shipmentDate, char shipmentType, string shipmentStatus, string notes, string carrierCode, string carrierDescription, string serviceCode, string paymentType, string transerMode, int totalPackageCount, double totalPackageWeight, List<ShipmentItemMovement> items)
    {
        Id = id;
        OrderId = orderId;
        SourceId = sourceId;
        OrderDate = orderDate;
        RequestDate = requestDate;
        ShipmentDate = shipmentDate;
        ShipmentType = shipmentType;
        ShipmentStatus = shipmentStatus;
        Notes = notes;
        CarrierCode = carrierCode;
        CarrierDescription = carrierDescription;
        ServiceCode = serviceCode;
        PaymentType = paymentType;
        TransferMode = transerMode;
        TotalPackageCount = totalPackageCount;
        TotalPackageWeight = totalPackageWeight;
        Items = items;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Shipment shipment)
        {
            // Sort the items lists before comparing them
            var sortedItems = Items?.OrderBy(i => i.ItemId).ThenBy(i => i.Amount).ToList();
            var sortedShipmentItems = shipment.Items?.OrderBy(i => i.ItemId).ThenBy(i => i.Amount).ToList();

            bool itemsAreTheSame = sortedItems != null && sortedShipmentItems != null &&
                       sortedItems.Count == sortedShipmentItems.Count &&
                       sortedItems.SequenceEqual(sortedShipmentItems);

            return shipment.Id == Id && shipment.OrderId == OrderId && shipment.SourceId == SourceId
            && shipment.OrderDate == OrderDate && shipment.RequestDate == RequestDate && shipment.ShipmentDate == ShipmentDate
            && shipment.ShipmentType == ShipmentType && shipment.ShipmentStatus == ShipmentStatus && shipment.Notes == Notes
            && shipment.CarrierCode == CarrierCode && shipment.CarrierDescription == CarrierDescription && shipment.ServiceCode == ServiceCode
            && shipment.PaymentType == PaymentType && shipment.TransferMode == TransferMode && shipment.TotalPackageCount == TotalPackageCount
            && shipment.TotalPackageWeight == TotalPackageWeight && itemsAreTheSame;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}