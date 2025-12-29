using DotNetExample.Domain.Commands;
using DotNetExample.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DotNetExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FooController : ControllerBase
{
    private readonly IGetFoo _getFoo;
    private readonly ISetFoo _setFoo;

    public FooController(IGetFoo getFoo, ISetFoo setFoo)
    {
        _getFoo = getFoo;
        _setFoo = setFoo;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _getFoo.ExecuteAsync();
        await _setFoo.ExecuteAsync(result);
        return Ok(result);
    }
}
