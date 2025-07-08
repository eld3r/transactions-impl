using Microsoft.AspNetCore.Mvc;
using Transactions.Services.Contracts;

namespace Transactions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController() : ControllerBase
{
    [HttpPost()]
    public async Task<ActionResult<SetTransactionResponse>> Set([FromBody] SetTransactionRequest request)
    {
        return new SetTransactionResponse();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetTransactionResponse>> Get()
    {
        return new GetTransactionResponse();
    }
}
