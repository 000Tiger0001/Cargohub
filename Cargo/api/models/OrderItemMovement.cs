using Newtonsoft.Json;

public class OrderItemMovement : ItemMovement
{
    [JsonProperty("Order_Id")]
    public int OrderId { get; set; }
}
