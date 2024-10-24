public class Items
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string UpcCode { get; set; }
    public string ModelNumber { get; set; }
    public string CommodityCode { get; set; }
    public Guid ItemLineId { get; set; }
    public Guid ItemGroupId { get; set; }
    public Guid ItemTypeId { get; set; }
    public int UnitPurchaseQuantity { get; set; }
    public int UnitOrderQuantity { get; set; }
    public int PackOrderQuantity { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierCode { get; set; }
    public string SupplierPartNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Items(string code, string description, string shortDescription, string upcCode, string modelNumber, string commodityCode, Guid itemLineId, Guid itemGroupId, Guid itemTypeId, int unitPurchaseQuantity, int unitOrderQuantity, int packOrderQuantity, Guid supplierId, string supplierCode, string supplierPartNumber, DateTime createdAt, DateTime updatedAt)
    {
        Id = Guid.NewGuid();
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
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}