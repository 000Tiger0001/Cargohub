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

    public User() { }

    public User(int id, string username, string password, string email, string address)
    {
        Id = id;
        Username = username;
        Password = password;
        Email = email;
        Address = address;
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
}