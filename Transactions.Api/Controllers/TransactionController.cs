using Microsoft.AspNetCore.Mvc;

namespace Transactions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController() : ControllerBase
{
    [HttpPost("")]
    public async Task<ActionResult> Set([FromBody] SetTransatcionRequest request)
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionResponse>> Get()
    {
        return new TransactionResponse();
    }
}
