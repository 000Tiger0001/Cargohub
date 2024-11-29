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
        // Configure entities and relationships here if needed
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasOne(i => i.Item)
                  .WithMany()
                  .HasForeignKey(i => i.ItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(oim => oim.Order)
            .HasForeignKey(oim => oim.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transfer>()
            .HasMany(o => o.Items)
            .WithOne(tim => tim.Transfer)
            .HasForeignKey(tim => tim.TransferId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.Items)
            .WithOne(sim => sim.Shipment)
            .HasForeignKey(sim => sim.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasOne(i => i.ItemLine)
                  .WithMany()
                  .HasForeignKey(i => i.ItemLineId)
                  .OnDelete(DeleteBehavior.SetNull); // Set to null if ItemLine is deleted

            entity.HasOne(i => i.ItemGroup)
                  .WithMany()
                  .HasForeignKey(i => i.ItemGroupId)
                  .OnDelete(DeleteBehavior.SetNull); // Set to null if ItemGroup is deleted

            entity.HasOne(i => i.ItemType)
                  .WithMany()
                  .HasForeignKey(i => i.ItemTypeId)
                  .OnDelete(DeleteBehavior.SetNull); // Set to null if ItemType is deleted

            entity.HasOne(i => i.Supplier)
                  .WithMany()
                  .HasForeignKey(i => i.SupplierId)
                  .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        });
    }
}