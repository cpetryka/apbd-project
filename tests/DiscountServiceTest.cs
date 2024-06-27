using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.EntityFrameworkCore;

namespace tests;

[TestClass]
public class DiscountServiceTest
{
    private DbContextOptions<ApplicationContext> _options;
    private ApplicationContext _context;
    private IDiscountService _discountService;

    // This method is called before each test method
    // [AssemblyInitialize]
    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "DiscountServiceTestDb")
            .Options;
        _context = new ApplicationContext(_options);
        _discountService = new DiscountService(_context);
    }

    // This method is called after each test method
    // [AssemblyCleanup]
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void GetDiscountById_WhenDiscountExists_ReturnsDiscount()
    {
        // Arrange
        var discount = new Discount
        {
            Id = 1,
            Name = "Discount 1",
            Offer = "Offer 1",
            Amount = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        _context.Discounts.Add(discount);
        _context.SaveChanges();

        // Act
        var result = _discountService.GetDiscountById(1).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(discount.Id, result.Id);
        Assert.AreEqual(discount.Name, result.Name);
        Assert.AreEqual(discount.Offer, result.Offer);
        Assert.AreEqual(discount.Amount, result.Amount);
        Assert.AreEqual(discount.StartDate, result.StartDate);
        Assert.AreEqual(discount.EndDate, result.EndDate);
    }

    [TestMethod]
    public void GetDiscountById_WhenDiscountDoesNotExist_ReturnsNull()
    {
        // Act
        var result = _discountService.GetDiscountById(1).Result;

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetAllDiscounts_WhenDiscountsExist_ReturnsDiscounts()
    {
        // Arrange
        var discount1 = new Discount
        {
            Id = 1,
            Name = "Discount 1",
            Offer = "Offer 1",
            Amount = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        var discount2 = new Discount
        {
            Id = 2,
            Name = "Discount 2",
            Offer = "Offer 2",
            Amount = 20,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        _context.Discounts.Add(discount1);
        _context.Discounts.Add(discount2);
        _context.SaveChanges();

        // Act
        var result = _discountService.GetAllDiscounts().Result;

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(discount1.Id, result.ElementAt(0).Id);
        Assert.AreEqual(discount1.Name, result.ElementAt(0).Name);
        Assert.AreEqual(discount1.Offer, result.ElementAt(0).Offer);
        Assert.AreEqual(discount1.Amount, result.ElementAt(0).Amount);
        Assert.AreEqual(discount1.StartDate, result.ElementAt(0).StartDate);
        Assert.AreEqual(discount1.EndDate, result.ElementAt(0).EndDate);
        Assert.AreEqual(discount2.Id, result.ElementAt(1).Id);
        Assert.AreEqual(discount2.Name, result.ElementAt(1).Name);
        Assert.AreEqual(discount2.Offer, result.ElementAt(1).Offer);
        Assert.AreEqual(discount2.Amount, result.ElementAt(1).Amount);
        Assert.AreEqual(discount2.StartDate, result.ElementAt(1).StartDate);
        Assert.AreEqual(discount2.EndDate, result.ElementAt(1).EndDate);
    }

    [TestMethod]
    public void GetAllDiscounts_WhenNoDiscountsExist_ReturnsEmptyList()
    {
        // Act
        var result = _discountService.GetAllDiscounts().Result;

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void AddDiscount_WhenDiscountIsValid_AddsDiscount()
    {
        // Arrange
        var addDiscountDto = new AddDiscountDto
        {
            Name = "Discount 1",
            Offer = "Offer 1",
            Amount = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        // Act
        _discountService.AddDiscount(addDiscountDto).Wait();

        // Assert
        var discount = _context.Discounts.First();
        Assert.AreEqual(1, discount.Id);
        Assert.AreEqual(addDiscountDto.Name, discount.Name);
        Assert.AreEqual(addDiscountDto.Offer, discount.Offer);
        Assert.AreEqual(addDiscountDto.Amount, discount.Amount);
        Assert.AreEqual(addDiscountDto.StartDate, discount.StartDate);
        Assert.AreEqual(addDiscountDto.EndDate, discount.EndDate);
    }

    [TestMethod]
    public void AddDiscount_WhenDiscountIsInvalid_ThrowsException()
    {
        // Arrange
        var addDiscountDto = new AddDiscountDto
        {
            Name = "Discount 1",
            Offer = "Offer 1",
            Amount = 10,
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now
        };

        // Act & Assert
        Assert.ThrowsExceptionAsync<DbUpdateException>(() => _discountService.AddDiscount(addDiscountDto));
    }

    [TestMethod]
    public void FindCurrentlyBestDiscount_WhenDiscountsExist_ReturnsBestDiscount()
    {
        // Arrange
        var discount1 = new Discount
        {
            Id = 1,
            Name = "Discount 1",
            Offer = "Offer 1",
            Amount = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        var discount2 = new Discount
        {
            Id = 2,
            Name = "Discount 2",
            Offer = "Offer 2",
            Amount = 20,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        _context.Discounts.Add(discount1);
        _context.Discounts.Add(discount2);
        _context.SaveChanges();

        // Act
        var result = _discountService.FindCurrentlyBestDiscount().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(discount2.Id, result.Id);
        Assert.AreEqual(discount2.Name, result.Name);
        Assert.AreEqual(discount2.Offer, result.Offer);
        Assert.AreEqual(discount2.Amount, result.Amount);
        Assert.AreEqual(discount2.StartDate, result.StartDate);
        Assert.AreEqual(discount2.EndDate, result.EndDate);
    }
}