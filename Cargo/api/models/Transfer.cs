public class Transfer : IHasId
{
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public Guid TransferFrom { get; set; }
    public Guid TransferTo { get; set; }
    public string TransferStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Item> Items { get; set; }

    public Transfer() { }
    public Transfer(string reference, Guid transferFrom, Guid transferTo, string transferStatus, List<Item> items)
    {
        Id = Guid.NewGuid();
        Reference = reference;
        TransferFrom = transferFrom;
        TransferTo = transferTo;
        TransferStatus = transferStatus;
        Items = items;
    }
}