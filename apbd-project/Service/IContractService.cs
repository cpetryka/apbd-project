using apbd_project.Model.Dto;

namespace apbd_project.Service;

public interface IContractService
{
    Task<bool> DoesContractExist(int id);
    Task<bool> IsContractDurationValid(DateTime startDate, DateTime endDate);
    Task<bool> DoesClientHaveAnyContracts(int id);
    Task<bool> DoesClientHaveContractForSoftwareProduct(int clientId, int softwareProductId);
    Task<int> AddOneTimePurchaseContract(AddOneTimePurchaseContractDto addContractDto);
}