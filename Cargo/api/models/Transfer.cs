using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Transfer : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("reference")]
    public string? Reference { get; set; }

    [JsonProperty("transfer_from")]
    public int? TransferFrom { get; set; } = null;

    [JsonProperty("transfer_to")]
    public int? TransferTo { get; set; } = null;

    [JsonProperty("transfer_status")]
    public string? TransferStatus { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [JsonProperty("items")]
    public List<TransferItemMovement>? Items { get; set; }

    public Transfer() { }
    public Transfer(int id, string reference, int transferFrom, int transferTo, string transferStatus, List<TransferItemMovement> items)
    {
        Id = id;
        Reference = reference;
        TransferFrom = transferFrom;
        TransferTo = transferTo;
        TransferStatus = transferStatus;
        Items = items;
    }
}