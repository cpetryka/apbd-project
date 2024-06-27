namespace apbd_project.Service;

public interface ICurrencyService
{
    Task<decimal?> GetCurrencyExchangeRate(string currencyCode);
}