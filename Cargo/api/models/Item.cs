public class Item : IHasId
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? UpcCode { get; set; }
    public string? ModelNumber { get; set; }
    public string? CommodityCode { get; set; }
    public int ItemLineId { get; set; }
    public int ItemGroupId { get; set; }
    public int ItemTypeId { get; set; }
    public int UnitPurchaseQuantity { get; set; }
    public int UnitOrderQuantity { get; set; }
    public int PackOrderQuantity { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierCode { get; set; }
    public string? SupplierPartNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Item() { }

    public Item(int id, string code, string description, string shortDescription, string upcCode, string modelNumber, string commodityCode, int itemLineId, int itemGroupId, int itemTypeId, int unitPurchaseQuantity, int unitOrderQuantity, int packOrderQuantity, int supplierId, string supplierCode, string supplierPartNumber)
    {
        Id = id;
        Code = code;
        Description = description;
        ShortDescription = shortDescription;
        UpcCode = upcCode;
        ModelNumber = modelNumber;
        CommodityCode = commodityCode;
        ItemLineId = itemLineId;
        ItemGroupId = itemGroupId;
        ItemTypeId = itemTypeId;
        UnitPurchaseQuantity = unitPurchaseQuantity;
        UnitOrderQuantity = unitOrderQuantity;
        PackOrderQuantity = packOrderQuantity;
        SupplierId = supplierId;
        SupplierCode = supplierCode;
        SupplierPartNumber = supplierPartNumber;
    }
}