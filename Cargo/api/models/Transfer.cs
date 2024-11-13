public class Transfer : IHasId
{
    public int Id { get; set; }
    public string Reference { get; set; }
    public int TransferFrom { get; set; }
    public int TransferTo { get; set; }
    public string TransferStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Item> Items { get; set; }

    public Transfer() { }
    public Transfer(int id, string reference, int transferFrom, int transferTo, string transferStatus, List<Item> items)
    {
        Id = id;
        Reference = reference;
        TransferFrom = transferFrom;
        TransferTo = transferTo;
        TransferStatus = transferStatus;
        Items = items;
    }
}