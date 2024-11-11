public class Client : IHasId
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public string ContactEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Client(string name, string address, string city, string zipCode, string province, string country, string contactName, string contactPhone, string contactemail)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        City = city;
        ZipCode = zipCode;
        Province = province;
        Country = country;
        ContactName = contactName;
        ContactPhone = contactPhone;
        ContactEmail = contactemail;
    }
}