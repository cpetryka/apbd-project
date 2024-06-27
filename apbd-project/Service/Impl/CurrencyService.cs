using System.Text.Json;

namespace apbd_project.Service.Impl;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;

    public CurrencyService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<decimal?> GetCurrencyExchangeRate(string currencyCode)
    {
        if (string.IsNullOrEmpty(currencyCode) || currencyCode == "PLN")
        {
            return 1;
        }

        // URLs for tables A and B
        const string tableAUrl = "http://api.nbp.pl/api/exchangerates/tables/A/?format=json";
        const string tableBUrl = "http://api.nbp.pl/api/exchangerates/tables/B/?format=json";

        try
        {
            // Get rates for tables A and B
            var tableARatesArray = JsonDocument
                .Parse(_httpClient.GetStringAsync(tableAUrl).Result)
                .RootElement[0]
                .GetProperty("rates")
                .EnumerateArray();

            var tableBRatesArray = JsonDocument
                .Parse(_httpClient.GetStringAsync(tableBUrl).Result)
                .RootElement[0]
                .GetProperty("rates")
                .EnumerateArray();

            // Combine rates from both tables
            var rates = new List<JsonElement>();
            rates.AddRange(tableARatesArray);
            rates.AddRange(tableBRatesArray);

            // Find the rate for the given currency code
            return rates
                .Where(r => r.GetProperty("code").GetString() == currencyCode)
                .Select(r => r.GetProperty("mid").GetDecimal())
                .FirstOrDefault();
        }
        catch (Exception e)
        {
            return 0;
        }
    }
}