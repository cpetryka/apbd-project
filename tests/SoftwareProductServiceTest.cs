using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.EntityFrameworkCore;

namespace tests;

[TestClass]
public class SoftwareProductServiceTest
{
    private DbContextOptions<ApplicationContext> _options;
    private ApplicationContext _context;
    private ISoftwareProductService _softwareProductService;

    // This method is called before each test method
    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "ClientServiceTestDb")
            .Options;
        _context = new ApplicationContext(_options);
        _softwareProductService = new SoftwareProductService(_context);
    }

    // This method is called after each test method
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void DoesSoftwareProductExist_WhenSoftwareProductExists_ReturnsTrue()
    {
        // Arrange
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

        // Act
        var result = _softwareProductService.DoesSoftwareProductExist(1).Result;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DoesSoftwareProductExist_WhenSoftwareProductDoesNotExist_ReturnsFalse()
    {
        // Act
        var result = _softwareProductService.DoesSoftwareProductExist(1).Result;

        // Assert
        Assert.IsFalse(result);
    }
}