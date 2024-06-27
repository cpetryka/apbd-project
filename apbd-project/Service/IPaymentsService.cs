namespace apbd_project.Service;

public interface IPaymentsService
{
    Task<bool> IsPaidAmountValid(int contractId, decimal amount);
    Task<bool> IsPaymentDateValid(int contractId, DateTime paymentDate);
    Task<decimal> GetPaidAmount(int contractId);
    Task<int> AddPaymentForOneTimePurchaseContract(int contractId, decimal amount);
    Task<decimal?> CalculateProfit(string currencyCode, int? softwareProductId);
    Task<decimal?> CalculatePredictableProfit(string currencyCode, int? softwareProductId, int months);
}