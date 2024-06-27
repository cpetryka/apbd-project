using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.EntityFrameworkCore;

namespace tests;

[TestClass]
public class PaymentsServiceTest
{
    private DbContextOptions<ApplicationContext> _options;
    private ApplicationContext _context;
    private IPaymentsService _paymentsService;

    // This method is called before each test method
    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "PaymentsServiceTestDb")
            .Options;
        _context = new ApplicationContext(_options);
        _paymentsService = new PaymentService(
            _context,
            new ContractService(_context, new DiscountService(_context)),
            new CurrencyService());
    }

    // This method is called after each test method
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public async Task IsPaidAmountValid_WhenAmountIsZero_ReturnsFalse()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.IsPaidAmountValid(1, 0);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsPaidAmountValid_WhenAmountIsCorrect_ReturnsTrue()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.IsPaidAmountValid(1, 50);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task IsPaidAmountValid_WhenAmountIsTooHigh_ReturnsFalse()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.IsPaidAmountValid(1, 150);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsPaymentDateValid_WhenDateIsBeforeContractStartDate_ReturnsFalse()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            StartDate = new DateTime(2022, 1, 1),
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = new DateTime(2022, 1, 31)
            }
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.IsPaymentDateValid(1, new DateTime(2021, 12, 31));

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsPaymentDateValid_WhenDateIsAfterContractEndDate_ReturnsFalse()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            StartDate = new DateTime(2022, 1, 1),
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = new DateTime(2022, 1, 31)
            }
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.IsPaymentDateValid(1, new DateTime(2022, 2, 1));

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task IsPaymentDateValid_WhenDateIsCorrect_ReturnsTrue()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            StartDate = new DateTime(2022, 1, 1),
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = new DateTime(2022, 1, 31)
            }
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.IsPaymentDateValid(1, new DateTime(2022, 1, 15));

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task GetPaidAmount_WhenNoPayments_ReturnsZero()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.GetPaidAmount(1);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task GetPaidAmount_WhenPaymentsExist_ReturnsSumOfPayments()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1
        };
        await _context.Contracts.AddAsync(contract);
        await _context.Payments.AddAsync(new Payment { ContractId = 1, Amount = 50 });
        await _context.Payments.AddAsync(new Payment { ContractId = 1, Amount = 25 });
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.GetPaidAmount(1);

        // Assert
        Assert.AreEqual(75, result);
    }

    /*[TestMethod]
    public async Task AddPaymentForOneTimePurchaseContract_WhenContractDoesNotExist_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50));
    }

    [TestMethod]
    public async Task AddPaymentForOneTimePurchaseContract_WhenAmountIsNotValid_ThrowsArgumentException()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _paymentsService.AddPaymentForOneTimePurchaseContract(1, 150));
    }

    [TestMethod]
    public async Task AddPaymentForOneTimePurchaseContract_WhenPaymentDateIsNotValid_ThrowsArgumentException()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            StartDate = new DateTime(2022, 1, 1),
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = new DateTime(2022, 1, 31)
            }
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50));
    }*/

    [TestMethod]
    public async Task AddPaymentForOneTimePurchaseContract_WhenDataIsValid_AddsPayment()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100,
            StartDate = DateTime.Now.AddDays(-2),
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = DateTime.Now.AddDays(14)
            }
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50);

        // Assert
        var payment = await _context.Payments.FirstOrDefaultAsync();
        Assert.IsNotNull(payment);
        Assert.AreEqual(1, payment.ContractId);
        Assert.AreEqual(50, payment.Amount);
    }

    [TestMethod]
    public async Task CalculateProfit_WhenNoPayments_ReturnsZero()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.CalculateProfit("USD", 1);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task CalculateProfit_WhenPaymentsExist_ReturnsProfit_v1()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.CalculateProfit("USD", null);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task CalculateProfit_WhenPaymentsExist_ReturnsProfit_v2()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100
        };
        await _context.Contracts.AddAsync(contract);

        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.CalculateProfit("PLN", null);

        // Assert
        Assert.AreEqual(100, result);
    }

    [TestMethod]
    public async Task CalculateProfit_WhenPaymentsForSpecificSoftwareProductExist_ReturnsProfit()
    {
        // Arrange
        var contract1 = new Contract
        {
            Id = 1,
            Price = 100,
            SoftwareProductId = 1
        };
        var contract2 = new Contract
        {
            Id = 2,
            Price = 200,
            SoftwareProductId = 2
        };
        await _context.Contracts.AddAsync(contract1);
        await _context.Contracts.AddAsync(contract2);

        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(2, 100);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.CalculateProfit("PLN", 1);

        // Assert
        Assert.AreEqual(100, result);
    }

    [TestMethod]
    public async Task CalculatePredictableProfit_WhenNoPayments_ReturnsZero()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100,
            SoftwareProductId = 1
        };
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.CalculatePredictableProfit("USD", 1, 12);

        // Assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public async Task CalculatePredictableProfit_WhenPaymentsExist_ReturnsProfit()
    {
        // Arrange
        var contract = new Contract
        {
            Id = 1,
            Price = 100,
            SoftwareProductId = 1
        };
        var contract2 = new Contract
        {
            Id = 2,
            Price = 100,
            SoftwareProductId = 1,
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = DateTime.Today.AddMonths(2)
            }
        };
        var contract3 = new Contract
        {
            Id = 3,
            Price = 100,
            SoftwareProductId = 2,
            OneTimePurchase = new OneTimePurchase
            {
                EndDate = DateTime.Today.AddMonths(2)
            }
        };

        await _context.Contracts.AddAsync(contract);
        await _context.Contracts.AddAsync(contract2);
        await _context.Contracts.AddAsync(contract3);

        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 50);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(1, 25);
        await _paymentsService.AddPaymentForOneTimePurchaseContract(2, 25);
        await _context.SaveChangesAsync();

        // Act
        var result = await _paymentsService.CalculatePredictableProfit("PLN", 1, 6);

        // Assert
        Assert.AreEqual(200, result);
    }
}