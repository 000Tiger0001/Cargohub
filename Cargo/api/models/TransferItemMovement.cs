using System.ComponentModel.DataAnnotations.Schema;

public class TransferItemMovement : ItemMovement
{
    public int TransferId { get; set; }

    [ForeignKey("TransferId")]
    public Transfer? Transfer { get; set; }
}
