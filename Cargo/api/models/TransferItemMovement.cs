using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

public class TransferItemMovement : ItemMovement
{
    [JsonProperty("transfer_id")]
    public int TransferId { get; set; }
}
