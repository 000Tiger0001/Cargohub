using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderItemMovement : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [JsonProperty("item_id")]
    public int ItemId { get; set; }

    [JsonProperty("amount")]
    public int Amount { get; set; }
}