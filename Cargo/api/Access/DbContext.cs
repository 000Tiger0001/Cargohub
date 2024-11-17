using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Define DbSet properties for each entity
    public DbSet<Client>? Clients { get; set; }
    public DbSet<Inventory>? Inventories { get; set; }
    public DbSet<Item>? Items { get; set; }
    public DbSet<ItemGroup>? ItemGroups { get; set; }
    public DbSet<ItemLine>? ItemLines { get; set; }
    public DbSet<ItemType>? ItemTypes { get; set; }
    public DbSet<Location>? Locations { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<Shipment>? Shipments { get; set; }
    public DbSet<Supplier>? Suppliers { get; set; }
    public DbSet<Transfer>? Transfers { get; set; }
    public DbSet<Warehouse>? Warehouses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().HasKey(o => o.Id);
        modelBuilder.Entity<Shipment>().HasKey(s => s.Id);

        modelBuilder.Entity<ItemMovement>().HasKey(im => im.Id);
        // Configure entities and relationships here if needed
        base.OnModelCreating(modelBuilder);
    }
}
