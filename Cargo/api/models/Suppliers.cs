public class Suppliers : IHasId
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string AddressExtra { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string ContactName { get; set; }
    public string Phonenumber { get; set; }
    public string Reference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Suppliers(string code, string name, string address, string addressExtra, string city, string zipCode, string province, string country, string contactName, string phonenumber, string reference)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Address = address;
        AddressExtra = addressExtra;
        City = city;
        ZipCode = zipCode;
        Province = province;
        Country = country;
        ContactName = contactName;
        Phonenumber = phonenumber;
        Reference = reference;
    }
}