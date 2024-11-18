using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderItemMovement : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [JsonProperty("item_id")]
    public string? ItemIdString { get; set; }

    // Convert the item_id string into an integer (ignoring the "P" prefix)
    [JsonIgnore]
    public int ItemId
    {
        get
        {
            // Check if the ItemIdString starts with "P" and convert to integer
            if (!string.IsNullOrEmpty(ItemIdString) && ItemIdString.StartsWith("P"))
            {
                string numericPart = ItemIdString.Substring(1); // Remove the "P" prefix
                if (int.TryParse(numericPart, out int numericId))
                {
                    return numericId;
                }
            }
            return 0; // Default value in case conversion fails
        }
        set
        {
            ItemIdString = "P" + value.ToString();
        }
    }

    [JsonProperty("amount")]
    public int Amount { get; set; }
}
