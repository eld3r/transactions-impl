using Microsoft.AspNetCore.Mvc;
using Transactions.Services;
using Transactions.Services.Contracts;

namespace Transactions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost()]
    public async Task<ActionResult<SetTransactionResponse>> SetAsync([FromBody] SetTransactionRequest request)
    {
        return await transactionService.CreateAsync(request);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetTransactionResponse>> GetAsync(Guid id)
    {
        return await transactionService.GetAsync(id);
    }
}
