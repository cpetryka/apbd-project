using apbd_project.Model.Dto;
using apbd_project.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apbd_project.Controller;

[ApiController]
[Route("api/contracts")]
[Authorize(Policy = "all")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly IClientService _clientService;
    private readonly ISoftwareProductService _softwareProductService;

    public ContractsController(IContractService contractService, IClientService clientService,
        ISoftwareProductService softwareProductService)
    {
        _contractService = contractService;
        _clientService = clientService;
        _softwareProductService = softwareProductService;
    }

    [HttpPost]
    public async Task<IActionResult> AddOneTimePurchaseContract(AddOneTimePurchaseContractDto addOneTimePurchaseContractDto)
    {
        // Check if client exists, if not return 404
        if (!await _clientService.DoesClientExist(addOneTimePurchaseContractDto.ClientId))
        {
            return NotFound(new
            {
                Message = "Client with id " + addOneTimePurchaseContractDto.ClientId + " does not exist"
            });
        }

        // Check if software product exists, if not return 404
        if (!await _softwareProductService.DoesSoftwareProductExist(addOneTimePurchaseContractDto.SoftwareProductId))
        {
            return NotFound(new
            {
                Message = "Software product with id " + addOneTimePurchaseContractDto.SoftwareProductId + " does not exist"
            });
        }

        // Check if contract duration is valid, if not return 400
        if (!await _contractService.IsContractDurationValid(addOneTimePurchaseContractDto.StartDate,
            addOneTimePurchaseContractDto.EndDate))
        {
            return BadRequest(new
            {
                Message = "Contract duration is not valid"
            });
        }

        // Check if client has a contract for the software product, if yes return 400
        if (await _contractService.DoesClientHaveContractForSoftwareProduct(addOneTimePurchaseContractDto.ClientId,
            addOneTimePurchaseContractDto.SoftwareProductId))
        {
            return BadRequest(new
            {
                Message = "Client already has a contract for the software product"
            });
        }

        // Add the contract
        var contractId = await _contractService.AddOneTimePurchaseContract(addOneTimePurchaseContractDto);
        return Created("api/contracts/" + contractId, new
        {
            Message = "Contract added successfully",
            ContractId = contractId
        });
    }
}