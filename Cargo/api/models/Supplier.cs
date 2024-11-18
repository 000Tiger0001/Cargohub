using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Supplier : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("address")]
    public string? Address { get; set; }

    [JsonProperty("address_extra")]
    public string? AddressExtra { get; set; }

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

    [JsonProperty("phonenumber")]
    public string? Phonenumber { get; set; }

    [JsonProperty("reference")]
    public string? Reference { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public Supplier() { }
    public Supplier(int id, string code, string name, string address, string addressExtra, string city, string zipCode, string province, string country, string contactName, string phonenumber, string reference)
    {
        Id = id;
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