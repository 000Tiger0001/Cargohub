public class Shipment : IHasId
{
    public Guid Id { get; set; }
    public List<Guid> OrderIds { get; set; }
    public int SourceId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime ShipmentDate { get; set; }
    public char ShipmentType { get; set; }
    public string ShipmentStatus { get; set; }
    public string Notes { get; set; }
    public string CarrierCode { get; set; }
    public string CarrierDescription { get; set; }
    public string ServiceCode { get; set; }
    public string PaymentType { get; set; }
    public string TransferMode { get; set; }
    public int TotalPackageCount { get; set; }
    public double TotalPackageWeight { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Dictionary<Guid, int> Items { get; set; }

    public Shipment(List<Guid> orderIds, int sourceId, DateTime orderDate, DateTime requestDate, DateTime shipmentDate, char shipmentType, string shipmentStatus, string notes, string carrierCode, string carrierDescription, string serviceCode, string paymentType, string transerMode, int totalPackageCount, double totalPackageWeight, Dictionary<Guid, int> items)
    {
        Id = Guid.NewGuid();
        OrderIds = orderIds;
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
}