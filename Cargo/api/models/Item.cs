using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Item : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [JsonProperty("uid")]
    public string? ItemIdString { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("short_description")]
    public string? ShortDescription { get; set; }

    [JsonProperty("upc_code")]
    public string? UpcCode { get; set; }

    [JsonProperty("model_number")]
    public string? ModelNumber { get; set; }

    [JsonProperty("commodity_code")]
    public string? CommodityCode { get; set; }

    [JsonProperty("item_line")]
    public int? ItemLineId { get; set; }

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("ItemLineId")]
    public virtual ItemLine? ItemLine { get; set; }

    [JsonProperty("item_group")]
    public int? ItemGroupId { get; set; }

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("ItemGroupId")]
    public virtual ItemGroup? ItemGroup { get; set; }

    [JsonProperty("item_type")]
    public int? ItemTypeId { get; set; }

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("ItemTypeId")]
    public virtual ItemType? ItemType { get; set; }

    [JsonProperty("unit_purchase_quantity")]
    public int UnitPurchaseQuantity { get; set; }

    [JsonProperty("unit_order_quantity")]
    public int UnitOrderQuantity { get; set; }

    [JsonProperty("pack_order_quantity")]
    public int PackOrderQuantity { get; set; }

    [JsonProperty("supplier_id")]
    public int SupplierId { get; set; }

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("SupplierId")]
    public virtual Supplier? Supplier { get; set; }

    [JsonProperty("supplier_code")]
    public string? SupplierCode { get; set; }

    [JsonProperty("supplier_part_number")]
    public string? SupplierPartNumber { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
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

    public override bool Equals(object? obj)
    {
        if (obj is Item item)
        {
            return item.Id == Id && item.Code == Code && item.Description == Description
            && item.ShortDescription == ShortDescription && item.UpcCode == UpcCode
            && item.ModelNumber == ModelNumber && item.CommodityCode == CommodityCode
            && item.ItemLineId == ItemLineId && item.ItemGroupId == ItemGroupId
            && item.ItemTypeId == ItemTypeId && item.UnitPurchaseQuantity == UnitPurchaseQuantity
            && item.UnitOrderQuantity == UnitOrderQuantity && item.PackOrderQuantity == PackOrderQuantity
            && item.SupplierId == SupplierId && item.SupplierCode == SupplierCode
            && item.SupplierPartNumber == SupplierPartNumber;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}