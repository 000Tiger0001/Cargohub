public class Warehouse : IHasId
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public string ContactEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Warehouse() { }
    public Warehouse(string code, string name, string address, string zip, string city, string province, string country, string contactName, string contactPhone, string contactEmail)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Address = address;
        Zip = zip;
        City = city;
        Province = province;
        Country = country;
        ContactName = contactName;
        ContactPhone = contactPhone;
        ContactEmail = contactEmail;
    }
}