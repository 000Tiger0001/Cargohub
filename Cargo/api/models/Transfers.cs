public class Transfers : IHasId
{
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public Guid TransferFrom { get; set; }
    public Guid TransferTo { get; set; }
    public string TransferStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Items> Items { get; set; }

    public Transfers(string reference, Guid transferFrom, Guid transferTo, string transferStatus, List<Items> items)
    {
        Id = Guid.NewGuid();
        Reference = reference;
        TransferFrom = transferFrom;
        TransferTo = transferTo;
        TransferStatus = transferStatus;
        Items = items;
    }
}