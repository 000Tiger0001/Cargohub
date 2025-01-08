using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class User : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("password")]
    public string? Password { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("address")]
    public string? Address { get; set; }

    [JsonProperty("role")]
    public string? Role { get; set; }

    [JsonProperty("warehouse")]
    public int? WarehouseId { get; set; }

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    [ForeignKey("WarehouseId")]
    public virtual Warehouse? Warehouse { get; set; }

    public User() { }

    public User(int id, string username, string password, string email, string address, string role)
    {
        Id = id;
        Username = username;
        Password = password;
        Email = email;
        Address = address;
        Role = role;
    }

    public override bool Equals(object? obj)
    {
        if (obj is User other)
        {
            return Id == other.Id && Username == other.Username
            && Address == other.Address && Password == other.Password
            && Email == other.Email;
        }
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public bool HasRight(List<string> rights)
    {
        if (rights.Contains(Role!)) return true;
        return false;
    }
}