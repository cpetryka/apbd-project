using apbd_project.Data;
using apbd_project.Model;
using apbd_project.Model.Dto;
using apbd_project.Service;
using apbd_project.Service.Impl;
using Microsoft.EntityFrameworkCore;

namespace tests;

[TestClass]
public class ClientServiceTest
{
    private DbContextOptions<ApplicationContext> _options;
    private ApplicationContext _context;
    private IClientService _clientService;

    // This method is called before each test method
    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "ClientServiceTestDb")
            .Options;
        _context = new ApplicationContext(_options);
        _clientService = new ClientService(_context);
    }

    // This method is called after each test method
    [TestCleanup]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [TestMethod]
    public void DoesClientExist_WhenClientExists_ReturnsTrue()
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            IndividualClient = new IndividualClient
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Pesel = "Pesel 1"
            }
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        var result = _clientService.DoesClientExist(1).Result;

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetClientById_WhenClientExists_ReturnsClient()
    {
        // Arrange
        var client = new Client
        {
            Id = 1,
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            IndividualClient = new IndividualClient
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Pesel = "Pesel 1",
                IsActive = true
            }
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        var result = _clientService.GetClientById(1).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(client.Id, result.Id);
        Assert.AreEqual(client.Address, result.Address);
        Assert.AreEqual(client.Email, result.Email);
        Assert.AreEqual(client.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(client.IndividualClient.Name, result.FirstName);
        Assert.AreEqual(client.IndividualClient.Surname, result.LastName);
        Assert.AreEqual(client.IndividualClient.Pesel, result.Pesel);
    }

    [TestMethod]
    public void GetClientById_WhenClientDoesNotExist_ReturnsNull()
    {
        // Act
        var result = _clientService.GetClientById(1).Result;

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetAllClients_WhenClientsExist_ReturnsClients()
    {
        // Arrange
        var client1 = new Client
        {
            Id = 1,
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            IndividualClient = new IndividualClient
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Pesel = "Pesel 1",
                IsActive = true
            }
        };
        var client2 = new Client
        {
            Id = 2,
            Address = "Address 2",
            Email = "Email 2",
            PhoneNumber = "PhoneNumber 2",
            BusinessClient = new BusinessClient
            {
                CompanyName = "CompanyName 2",
                KrsNumber = "KrsNumber 2"
            }
        };
        _context.Clients.Add(client1);
        _context.Clients.Add(client2);
        _context.SaveChanges();

        // Act
        var result = _clientService.GetAllClients().Result;

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetAllClients_WhenClientsDoNotExist_ReturnsEmptyList()
    {
        // Act
        var result = _clientService.GetAllClients().Result;

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void AddIndividualClient_WhenClientDoesNotExist_AddsClient()
    {
        // Arrange
        var addIndividualClientDto = new AddIndividualClientDto
        {
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            FirstName = "Name 1",
            LastName = "Surname 1",
            Pesel = "Pesel 1"
        };

        // Act
        _clientService.AddIndividualClient(addIndividualClientDto).Wait();

        // Assert
        Assert.AreEqual(1, _context.Clients.Count());
        var client = _context.Clients.First();
        Assert.AreEqual(addIndividualClientDto.Address, client.Address);
        Assert.AreEqual(addIndividualClientDto.Email, client.Email);
        Assert.AreEqual(addIndividualClientDto.PhoneNumber, client.PhoneNumber);
        Assert.IsNotNull(client.IndividualClient);
        Assert.AreEqual(addIndividualClientDto.FirstName, client.IndividualClient.Name);
        Assert.AreEqual(addIndividualClientDto.LastName, client.IndividualClient.Surname);
        Assert.AreEqual(addIndividualClientDto.Pesel, client.IndividualClient.Pesel);
    }

    [TestMethod]
    public void AddBusinessClient_WhenClientDoesNotExist_AddsClient()
    {
        // Arrange
        var addBusinessClientDto = new AddBusinessClientDto
        {
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            CompanyName = "CompanyName 1",
            KrsNumber = "KrsNumber 1"
        };

        // Act
        _clientService.AddBusinessClient(addBusinessClientDto).Wait();

        // Assert
        Assert.AreEqual(1, _context.Clients.Count());
        var client = _context.Clients.First();
        Assert.AreEqual(addBusinessClientDto.Address, client.Address);
        Assert.AreEqual(addBusinessClientDto.Email, client.Email);
        Assert.AreEqual(addBusinessClientDto.PhoneNumber, client.PhoneNumber);
        Assert.IsNotNull(client.BusinessClient);
        Assert.AreEqual(addBusinessClientDto.CompanyName, client.BusinessClient.CompanyName);
        Assert.AreEqual(addBusinessClientDto.KrsNumber, client.BusinessClient.KrsNumber);
    }

    [TestMethod]
    public void UpdateIndividualClient_WhenClientExists_UpdatesClient()
    {
        // Arrange
        var client = new Client
        {
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            IndividualClient = new IndividualClient
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Pesel = "Pesel 1"
            }
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        var updateIndividualClientDto = new UpdateIndividualClientDto
        {
            Address = "Address 2",
            Email = "Email 2",
            PhoneNumber = "PhoneNumber 2",
            FirstName = "Name 2",
            LastName = "Surname 2"
        };

        // Act
        _clientService.UpdateIndividualClient(client.Id, updateIndividualClientDto).Wait();

        // Assert
        var updatedClient = _context.Clients.First();
        Assert.AreEqual(updateIndividualClientDto.Address, updatedClient.Address);
        Assert.AreEqual(updateIndividualClientDto.Email, updatedClient.Email);
        Assert.AreEqual(updateIndividualClientDto.PhoneNumber, updatedClient.PhoneNumber);
        Assert.AreEqual(updateIndividualClientDto.FirstName, updatedClient.IndividualClient.Name);
        Assert.AreEqual(updateIndividualClientDto.LastName, updatedClient.IndividualClient.Surname);
    }

    [TestMethod]
    public void UpdateIndividualClient_WhenClientDoesNotExist_ThrowsException()
    {
        // Arrange
        var updateIndividualClientDto = new UpdateIndividualClientDto
        {
            Address = "Address 2",
            Email = "Email 2",
            PhoneNumber = "PhoneNumber 2",
            FirstName = "Name 2",
            LastName = "Surname 2"
        };

        // Act and assert
        Assert.ThrowsExceptionAsync<Exception>(() => _clientService.UpdateIndividualClient(1, updateIndividualClientDto));
    }

    [TestMethod]
    public void UpdateBusinessClient_WhenClientExists_UpdatesClient()
    {
        // Arrange
        var client = new Client
        {
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            BusinessClient = new BusinessClient
            {
                CompanyName = "CompanyName 1",
                KrsNumber = "KrsNumber 1"
            }
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        var updateBusinessClientDto = new UpdateBusinessClientDto
        {
            Address = "Address 2",
            Email = "Email 2",
            PhoneNumber = "PhoneNumber 2",
            CompanyName = "CompanyName 2"
        };

        // Act
        _clientService.UpdateBusinessClient(client.Id, updateBusinessClientDto).Wait();

        // Assert
        var updatedClient = _context.Clients.First();
        Assert.AreEqual(updateBusinessClientDto.Address, updatedClient.Address);
        Assert.AreEqual(updateBusinessClientDto.Email, updatedClient.Email);
        Assert.AreEqual(updateBusinessClientDto.PhoneNumber, updatedClient.PhoneNumber);
        Assert.AreEqual(updateBusinessClientDto.CompanyName, updatedClient.BusinessClient.CompanyName);
    }

    [TestMethod]
    public void UpdateBusinessClient_WhenClientDoesNotExist_ThrowsException()
    {
        // Arrange
        var updateBusinessClientDto = new UpdateBusinessClientDto
        {
            Address = "Address 2",
            Email = "Email 2",
            PhoneNumber = "PhoneNumber 2",
            CompanyName = "CompanyName 2"
        };

        // Act and assert
        Assert.ThrowsExceptionAsync<Exception>(() => _clientService.UpdateBusinessClient(1, updateBusinessClientDto));
    }

    [TestMethod]
    public void DeleteClient_WhenClientExists_SoftDeletesClient()
    {
        // Arrange
        var client = new Client
        {
            Address = "Address 1",
            Email = "Email 1",
            PhoneNumber = "PhoneNumber 1",
            IndividualClient = new IndividualClient
            {
                Name = "Name 1",
                Surname = "Surname 1",
                Pesel = "Pesel 1"
            }
        };
        _context.Clients.Add(client);
        _context.SaveChanges();

        // Act
        _clientService.DeleteClient(client.Id).Wait();

        // Assert
        var deletedClient = _context.Clients.First();
        Assert.IsFalse(deletedClient.IndividualClient.IsActive);
    }

    [TestMethod]
    public void DeleteClient_WhenClientDoesNotExist_ThrowsException()
    {
        // Act and assert
        Assert.ThrowsExceptionAsync<Exception>(() => _clientService.DeleteClient(1));
    }
}