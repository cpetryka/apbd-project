using apbd_project.Data;
using apbd_project.Service;
using apbd_project.Service.Impl;

namespace tests;

[TestClass]
public class CurrencyServiceTest
{
    private readonly ICurrencyService _currencyService;

    public CurrencyServiceTest()
    {
        _currencyService = new CurrencyService();
    }

    [TestMethod]
    public void GetCurrencyExchangeRate_WhenCurrencyCodeIsPLN_ReturnOne()
    {
        // Arrange
        var currencyCode = "PLN";

        // Act
        var result = _currencyService.GetCurrencyExchangeRate(currencyCode);

        // Assert
        Assert.AreEqual(1, result.Result);
    }

    [TestMethod]
    public void GetCurrencyExchangeRate_WhenCurrencyCodeIsCorrect_ReturnNotNull()
    {
        // Arrange
        var currencyCode = "USD";

        // Act
        var result = _currencyService.GetCurrencyExchangeRate(currencyCode);

        // Assert
        Assert.IsNotNull(result.Result);
    }

    [TestMethod]
    public void GetCurrencyExchangeRate_WhenCurrencyCodeIsIncorrect_ReturnZero()
    {
        // Arrange
        var currencyCode = "XYZ";

        // Act
        var result = _currencyService.GetCurrencyExchangeRate(currencyCode);

        // Assert
        Assert.AreEqual(0, result.Result);
    }
}