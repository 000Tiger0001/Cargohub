using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ItemGroup : IHasId
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public ItemGroup() { }

    public ItemGroup(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ItemGroup other) return Id == other.Id && Name == other.Name && Description == other.Description;
        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();
}