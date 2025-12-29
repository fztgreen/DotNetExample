// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace DotNetExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FooController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello from FooController!");
    }
}
