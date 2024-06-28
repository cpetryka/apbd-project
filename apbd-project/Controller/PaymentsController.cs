using apbd_project.Model.Dto;
using apbd_project.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apbd_project.Controller;

[ApiController]
[Route("api/payments")]
[Authorize(Policy = "all")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentsService _paymentsService;
    private readonly IContractService _contractService;
    private readonly ISoftwareProductService _softwareProductService;

    public PaymentsController(IPaymentsService paymentsService, IContractService contractService,
        ISoftwareProductService softwareProductService)
    {
        _paymentsService = paymentsService;
        _contractService = contractService;
        _softwareProductService = softwareProductService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPaymentForOneTimePurchaseContract(AddPaymentForOneTimePurchaseContractDto addPaymentDto)
    {
        if (!await _contractService.DoesContractExist(addPaymentDto.ContractId))
        {
            return NotFound("Contract does not exist");
        }

        if (!await _paymentsService.IsPaidAmountValid(addPaymentDto.ContractId, addPaymentDto.Amount))
        {
            return BadRequest("Amount is not valid");
        }

        if (!await _paymentsService.IsPaymentDateValid(addPaymentDto.ContractId, DateTime.Now))
        {
            return BadRequest("Payment date is not valid");
        }

        var paymentId = await _paymentsService.AddPaymentForOneTimePurchaseContract(addPaymentDto.ContractId, addPaymentDto.Amount);

        return Created("", new
        {
            Message = "Payment added successfully",
            PaymentId = paymentId
        });
    }

    [HttpGet("actual-profit")]
    public async Task<IActionResult> CalculateProfit(string currencyCode, int? softwareProductId)
    {
        if (softwareProductId != null && !await _softwareProductService.DoesSoftwareProductExist(softwareProductId.Value))
        {
            return NotFound("Software product does not exist");
        }

        var profit = await _paymentsService.CalculateProfit(currencyCode, softwareProductId);

        return Ok(new
        {
            Profit = profit
        });
    }

    [HttpGet("predictable-profit")]
    public async Task<IActionResult> CalculatePredictableProfit(string currencyCode, int? softwareProductId, int months)
    {
        if (softwareProductId != null && !await _softwareProductService.DoesSoftwareProductExist(softwareProductId.Value))
        {
            return NotFound("Software product does not exist");
        }

        var profit = await _paymentsService.CalculatePredictableProfit(currencyCode, softwareProductId, months);

        return Ok(new
        {
            Profit = profit
        });
    }
}