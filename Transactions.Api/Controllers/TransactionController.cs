using Microsoft.AspNetCore.Mvc;
using Transactions.Api.Contracts;
using Transactions.Api.Services;

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
