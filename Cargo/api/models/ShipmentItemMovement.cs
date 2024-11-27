using System.ComponentModel.DataAnnotations.Schema;

public class ShipmentItemMovement : ItemMovement
{
    public int ShipmentId { get; set; }

    [ForeignKey("ShipmentId")]
    public Shipment? Shipment { get; set; }
}
