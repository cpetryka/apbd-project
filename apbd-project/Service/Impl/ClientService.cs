using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace apbd_project.Service.Impl;

public class ClientService : IClientService
{
    private readonly ApplicationContext _context;

    public ClientService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<bool> DoesClientExist(int id)
    {
        return await _context.Clients.AnyAsync(c => c.Id == id);
    }

    public async Task<GetClientDto?> GetClientById(int id)
    {
        return await _context
            .Clients
            .Where(c => c.Id == id)
            .Select(c => new GetClientDto
            {
                Id = c.Id,
                Address = c.Address,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                FirstName = c.IndividualClient.Name,
                LastName = c.IndividualClient.Surname,
                Pesel = c.IndividualClient.Pesel,
                CompanyName = c.BusinessClient.CompanyName,
                KrsNumber = c.BusinessClient.KrsNumber
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GetClientDto>> GetAllClients()
    {
        return await _context
            .Clients
            .Include(c => c.IndividualClient)
            .Include(c => c.BusinessClient)
            .Where(c => c.IndividualClient == null || c.IndividualClient.IsActive)
            .Select(c => new GetClientDto
            {
                Id = c.Id,
                Address = c.Address,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                FirstName = c.IndividualClient.Name,
                LastName = c.IndividualClient.Surname,
                Pesel = c.IndividualClient.Pesel,
                CompanyName = c.BusinessClient.CompanyName,
                KrsNumber = c.BusinessClient.KrsNumber
            })
            .ToListAsync();
    }

    public async Task AddIndividualClient(AddIndividualClientDto addIndividualClientDto)
    {
        // Add client
        var client = new Client
        {
            Address = addIndividualClientDto.Address,
            Email = addIndividualClientDto.Email,
            PhoneNumber = addIndividualClientDto.PhoneNumber
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        // Add individual client
        var individualClient = new IndividualClient
        {
            ClientId = client.Id,
            Name = addIndividualClientDto.FirstName,
            Surname = addIndividualClientDto.LastName,
            Pesel = addIndividualClientDto.Pesel,
            IsActive = true
        };

        client.IndividualClient = individualClient;

        await _context.IndividualClients.AddAsync(individualClient);
        await _context.SaveChangesAsync();
    }

    public async Task AddBusinessClient(AddBusinessClientDto addBusinessClientDto)
    {
        // Add client
        var client = new Client
        {
            Address = addBusinessClientDto.Address,
            Email = addBusinessClientDto.Email,
            PhoneNumber = addBusinessClientDto.PhoneNumber
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        // Add business client
        var businessClient = new BusinessClient
        {
            ClientId = client.Id,
            CompanyName = addBusinessClientDto.CompanyName,
            KrsNumber = addBusinessClientDto.KrsNumber
        };

        client.BusinessClient = businessClient;

        await _context.BusinessClients.AddAsync(businessClient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateIndividualClient(int id, UpdateIndividualClientDto updateIndividualClientDto)
    {
        // Update individual client data
        var individualClient = await _context.IndividualClients.FindAsync(id);

        if (individualClient == null)
        {
            throw new ArgumentException("Client not found");
        }

        individualClient.Name = updateIndividualClientDto.FirstName;
        individualClient.Surname = updateIndividualClientDto.LastName;

        // update client data
        var client = await _context.Clients.FindAsync(id);

        client.Address = updateIndividualClientDto.Address;
        client.Email = updateIndividualClientDto.Email;
        client.PhoneNumber = updateIndividualClientDto.PhoneNumber;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateBusinessClient(int id, UpdateBusinessClientDto updateBusinessClientDto)
    {
        // Update business client data
        var businessClient = await _context.BusinessClients.FindAsync(id);

        if (businessClient == null)
        {
            throw new ArgumentException("Client not found");
        }

        businessClient.CompanyName = updateBusinessClientDto.CompanyName;

        // update client data
        var client = await _context.Clients.FindAsync(id);

        client.Address = updateBusinessClientDto.Address;
        client.Email = updateBusinessClientDto.Email;
        client.PhoneNumber = updateBusinessClientDto.PhoneNumber;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteClient(int id)
    {
        var client = await _context.Clients
            .Include(c => c.IndividualClient)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (client == null)
        {
            throw new ArgumentException("Client not found");
        }

        // If client is an individual client, set IsActive to false
        if (client.IndividualClient != null)
        {
            client.IndividualClient.IsActive = false;
            await _context.SaveChangesAsync();
        }
        // If client is a business client, throw an exception, because business clients can't be deleted
        else
        {
            throw new ArgumentException("Business clients can't be deleted");
        }
    }
}