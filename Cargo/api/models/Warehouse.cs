using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class Warehouse : IHasId
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("address")]
    public string? Address { get; set; }

    [JsonProperty("zip")]
    public string? Zip { get; set; }

    [JsonProperty("city")]
    public string? City { get; set; }

    [JsonProperty("province")]
    public string? Province { get; set; }

    [JsonProperty("country")]
    public string? Country { get; set; }

    // Use a custom property to handle the nested 'contact' object
    [JsonProperty("contact")]
    private JObject? ContactJson { get; set; }

    // This will expose the 'name' from the 'contact' field
    public string? ContactName
    {
        get => ContactJson?["name"]?.ToString();
        set
        {
            if (ContactJson == null)
            {
                ContactJson = new JObject();
            }

            ContactJson["name"] = value;
        }
    }

    public string? ContactPhone
    {
        get => ContactJson?["phone"]?.ToString();
        set
        {
            if (ContactJson == null)
            {
                ContactJson = new JObject();
            }

            ContactJson["phone"] = value;
        }
    }

    public string? ContactEmail
    {
        get => ContactJson?["email"]?.ToString();
        set
        {
            if (ContactJson == null)
            {
                ContactJson = new JObject();
            }

            ContactJson["email"] = value;
        }
    }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Warehouse() { }
    public Warehouse(string id, string code, string name, string address, string zip, string city, string province, string country, string contactName, string contactPhone, string contactEmail)
    {
        Id = id;
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


