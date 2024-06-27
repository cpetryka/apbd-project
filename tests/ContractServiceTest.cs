using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.EntityFrameworkCore;

namespace tests;

[TestClass]
public class ContractServiceTest
{
    private DbContextOptions<ApplicationContext> _options;
    private ApplicationContext _context;
    private IContractService _contractService;

    // This method is called before each test method
    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "ContractServiceTestDb")
            .Options;
        _context = new ApplicationContext(_options);
        _contractService = new ContractService(_context, new DiscountService(_context));
    }

    // This method is called after each test method
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void IsContractDurationValid_WhenContractDurationIsValid_ReturnsTrue()
    {
        Assert.IsTrue(_contractService.IsContractDurationValid(DateTime.Now, DateTime.Now.AddDays(14)).Result);
    }

    [TestMethod]
    public void IsContractDurationValid_WhenContractDurationIsInvalid_ReturnsFalse()
    {
        Assert.IsFalse(_contractService.IsContractDurationValid(DateTime.Now, DateTime.Now.AddDays(40)).Result);
    }

    [TestMethod]
    public void DoesClientHaveAnyContracts_WhenClientHasContracts_ReturnsTrue()
    {
        // Arrange and Act
        _context.Contracts.Add(new Contract { ClientId = 1 });
        _context.SaveChanges();

        // Assert
        Assert.IsTrue(_contractService.DoesClientHaveAnyContracts(1).Result);
    }

    [TestMethod]
    public void DoesClientHaveAnyContracts_WhenClientDoesNotHaveContracts_ReturnsFalse()
    {
        Assert.IsFalse(_contractService.DoesClientHaveAnyContracts(1).Result);
    }

    [TestMethod]
    public void DoesClientHaveContractForSoftwareProduct_WhenClientHasContractForSoftwareProduct_ReturnsTrue()
    {
        // Arrange and Act
        _context.Contracts.Add(new Contract { ClientId = 1, SoftwareProductId = 1 });
        _context.SaveChanges();

        // Assert
        Assert.IsTrue(_contractService.DoesClientHaveContractForSoftwareProduct(1, 1).Result);
    }

    [TestMethod]
    public void DoesClientHaveContractForSoftwareProduct_WhenClientDoesNotHaveContractForSoftwareProduct_ReturnsFalse()
    {
        Assert.IsFalse(_contractService.DoesClientHaveContractForSoftwareProduct(1, 1).Result);
    }

    [TestMethod]
    public void AddOneTimePurchaseContract_WhenClientDoesNotHaveAContractForGivenSoftwareProduct_ReturnsTrue()
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            IndividualClient = new IndividualClient
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Pesel = "Pesel 1"
            }
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        var softwareProduct = new SoftwareProduct
        {
            Id = 1,
            Name = "Software Product 1",
            Description = "Description 1",
            Version = "1.0",
            Category = "Education",
            Type = "One-time purchase"
        };
        _context.SoftwareProducts.Add(softwareProduct);
        _context.SaveChanges();

        var addContractDto = new AddOneTimePurchaseContractDto
        {
            ClientId = 1,
            SoftwareProductId = 1,
            Price = 10_000,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(14),
            Version = "1.0",
            SupportEndDate = DateTime.Now.AddYears(1)
        };

        // Act
        var contractId = _contractService.AddOneTimePurchaseContract(addContractDto).Result;

        // Assert
        Assert.AreEqual(1, _context.Contracts.Count());
        Assert.AreEqual(1, _context.OneTimePurchases.Count());
    }
}