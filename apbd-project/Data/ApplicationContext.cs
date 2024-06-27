using apbd_project.Model;
using Microsoft.EntityFrameworkCore;

namespace apbd_project.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<IndividualClient> IndividualClients { get; set; } = null!;
    public DbSet<BusinessClient> BusinessClients { get; set; } = null!;
    public DbSet<SoftwareProduct> SoftwareProducts { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;
    public DbSet<Contract> Contracts { get; set; } = null!;
    public DbSet<OneTimePurchase> OneTimePurchases { get; set; } = null!;

    public ApplicationContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Id = 1,
                Address = "ul. Klonowa 2, 00-001 Warszawa",
                Email = "client1@gmail.com",
                PhoneNumber = "123456789"
            },
            new Client
            {
                Id = 2,
                Address = "ul. Brzozowa 3, 00-002 Warszawa",
                Email = "client2@gmail.com",
                PhoneNumber = "987654321"
            }
        );

        modelBuilder.Entity<IndividualClient>().HasData(
            new IndividualClient
            {
                ClientId = 1,
                Name = "Jan",
                Surname = "Kowalski",
                Pesel = "12345678901",
                IsActive = true
            }
        );

        modelBuilder.Entity<BusinessClient>().HasData(
            new BusinessClient
            {
                ClientId = 2,
                CompanyName = "Firma XYZ",
                KrsNumber = "9876543210"
            }
        );

        modelBuilder.Entity<SoftwareProduct>().HasData(
            new SoftwareProduct
            {
                Id = 1,
                Name = "Software product 1",
                Description = "Some description",
                Version = "1.0",
                Category = "Finance",
                Type = "Subscription"
            },
            new SoftwareProduct
            {
                Id = 2,
                Name = "Product 2",
                Description = "Description 2",
                Version = "2.0",
                Category = "Education",
                Type = "One-time purchase"
            }
        );

        modelBuilder.Entity<Discount>().HasData(
            new Discount
            {
                Id = 1,
                Name = "Some discount",
                Offer = "Some offer",
                Amount = 15,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            }
        );

        // Configure the precision and scale for the Price property
        modelBuilder.Entity<Contract>().Property(c => c.Price).HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                ClientId = 1,
                SoftwareProductId = 1,
                Price = 10_000,
                StartDate = DateTime.Now,
                SignedDate = DateTime.Now
            }
        );

        modelBuilder.Entity<OneTimePurchase>().HasData(
            new OneTimePurchase
            {
                ContractId = 1,
                EndDate = DateTime.Now.AddDays(14),
                Version = "1.0",
                SupportEndDate = DateTime.Now.AddYears(1)
            }
        );
    }
}