using apbd_project.Model.Dto;
using apbd_project.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apbd_project.Controller;

[ApiController]
[Route("api/clients")]
[Authorize(Policy = "all")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClientById(int id)
    {
        var client = await _clientService.GetClientById(id);

        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var clients = await _clientService.GetAllClients();
        return Ok(clients);
    }

    [HttpPost("individual")]
    public async Task<IActionResult> AddIndividualClient(AddIndividualClientDto addIndividualClientDto)
    {
        await _clientService.AddIndividualClient(addIndividualClientDto);

        return Created("api/clients/individual", new
        {
            Message = "Individual client added successfully."
        });
    }

    [HttpPost("business")]
    public async Task<IActionResult> AddBusinessClient(AddBusinessClientDto addBusinessClientDto)
    {
        await _clientService.AddBusinessClient(addBusinessClientDto);
        return Created("api/clients/business", new
        {
            Message = "Business client added successfully."
        });
    }

    [HttpPatch("individual/{id}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateIndividualClient(int id, UpdateIndividualClientDto updateIndividualClientDto)
    {
        await _clientService.UpdateIndividualClient(id, updateIndividualClientDto);
        return Ok(new
        {
            Message = "Individual client updated successfully."
        });
    }

    [HttpPatch("business/{id}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> UpdateBusinessClient(int id, UpdateBusinessClientDto updateBusinessClientDto)
    {
        await _clientService.UpdateBusinessClient(id, updateBusinessClientDto);
        return Ok(new
        {
            Message = "Business client updated successfully."
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        await _clientService.DeleteClient(id);
        return Ok(new
        {
            Message = "Client deleted successfully."
        });
    }
}