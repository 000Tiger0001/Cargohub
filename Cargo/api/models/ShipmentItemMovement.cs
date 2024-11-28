using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class ShipmentItemMovement : ItemMovement
{
    [JsonProperty("shipment_id")]
    public int ShipmentId { get; set; }
}
