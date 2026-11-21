using DotNetExample.Domain.Commands.Foo;
using DotNetExample.Domain.Queries.Foo;

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
        var result = await _getFoo.CurrentAsync();
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] string value)
    {
        await _setFoo.ToAsync(value);
        return NoContent();
    }
}
