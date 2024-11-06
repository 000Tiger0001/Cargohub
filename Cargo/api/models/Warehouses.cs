public class Warehouses
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public Dictionary<string, string> Contact { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Warehouses(string code, string name, string address, string zip, string city, string province, string country, Dictionary<string, string> contact)
    {
        Code = code;
        Name = name;
        Address = address;
        Zip = zip;
        City = city;
        Province = province;
        Country = country;
        Contact = contact;
    }
}