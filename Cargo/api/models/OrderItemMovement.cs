using System.ComponentModel.DataAnnotations.Schema;

public class OrderItemMovement : ItemMovement
{
    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }
}
