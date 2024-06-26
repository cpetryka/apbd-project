using apbd_project.Model;
using Microsoft.EntityFrameworkCore;

namespace apbd_project.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<IndividualClient> IndividualClients { get; set; } = null!;
    public DbSet<BusinessClient> BusinessClients { get; set; } = null!;

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

        /*modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Address).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired();
        });

        modelBuilder.Entity<IndividualClient>(entity =>
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Surname).IsRequired();
            entity.Property(e => e.Pesel).IsRequired().HasMaxLength(11);
            entity.Property(e => e.IsActive).IsRequired();
            entity.HasOne(e => e.Client).WithOne().HasForeignKey<IndividualClient>(e => e.ClientId);
        });

        modelBuilder.Entity<BusinessClient>(entity =>
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.CompanyName).IsRequired();
            entity.Property(e => e.KrsNumber).IsRequired().HasMaxLength(10);
            entity.HasOne(e => e.Client).WithOne().HasForeignKey<BusinessClient>(e => e.ClientId);
        });*/
    }
}