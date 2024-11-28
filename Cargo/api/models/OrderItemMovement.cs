using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

public class OrderItemMovement : ItemMovement
{
    [JsonProperty("order_id")]
    public int OrderId { get; set; }
}
