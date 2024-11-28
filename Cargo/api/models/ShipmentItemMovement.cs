using System.ComponentModel.DataAnnotations.Schema;

public class ShipmentItemMovement : ItemMovement
{
    public int ShipmentId { get; set; }

    [ForeignKey("ShipmentId")]
    public virtual Shipment? Shipment { get; set; }
}
