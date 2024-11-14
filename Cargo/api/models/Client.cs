using Newtonsoft.Json;

public class Client : IHasId
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("address")]
    public string? Address { get; set; }

    [JsonProperty("city")]
    public string? City { get; set; }

    [JsonProperty("zip_code")]
    public string? ZipCode { get; set; }

    [JsonProperty("province")]
    public string? Province { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }

    [JsonProperty("contact_name")]
    public string? ContactName { get; set; }

    [JsonProperty("contact_phone")]
    public string? ContactPhone { get; set; }

    [JsonProperty("contact_email")]
    public string? ContactEmail { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Client() { }

    public Client(int id, string name, string address, string city, string zipCode, string? province, string country, string contactName, string contactPhone, string contactemail)
    {
        Id = id;
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