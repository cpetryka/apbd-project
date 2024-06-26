using apbd_project.Model;
using apbd_project.Model.Dto;

namespace apbd_project.Service;

public interface IClientService
{
    Task<bool> DoesClientExist(int id);
    Task<GetClientDto?> GetClientById(int id);
    Task<IEnumerable<GetClientDto>> GetAllClients();
    Task AddIndividualClient(AddIndividualClientDto addIndividualClientDto);
    Task AddBusinessClient(AddBusinessClientDto addBusinessClientDto);
    Task UpdateIndividualClient(int id, UpdateIndividualClientDto updateIndividualClientDto);
    Task UpdateBusinessClient(int id, UpdateBusinessClientDto updateBusinessClientDto);
    Task DeleteClient(int id);
}