using DotNetExample.Api.Models.Cars;
using DotNetExample.Domain.Commands.Cars;
using DotNetExample.Domain.Exceptions.Cars;
using DotNetExample.Domain.Queries.Cars;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetExample.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly IGetCar _getCar;
    private readonly IGetCars _getCars;
    private readonly IAddCar _addCar;
    private readonly IUpdateCar _updateCar;
    private readonly IDeleteCar _deleteCar;

    public CarsController(IGetCar getCar, IGetCars getCars, IAddCar addCar, IUpdateCar updateCar, IDeleteCar deleteCar)
    {
        _getCar = getCar;
        _getCars = getCars;
        _addCar = addCar;
        _updateCar = updateCar;
        _deleteCar = deleteCar;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CarResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _getCars.AllAsync();
        return Ok(cars.Select(CarResponse.From).ToArray());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<CarResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var car = await _getCar.ByAsync(id);
        return car is null ? NotFound() : Ok(CarResponse.From(car));
    }

    [HttpPost]
    [ProducesResponseType<CarResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CarRequest request)
    {
        var car = request.ToCar(Guid.NewGuid());
        await _addCar.WithAsync(car);
        return CreatedAtAction(nameof(Get), new CarRouteValues(car.Id), CarResponse.From(car));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, CarRequest request)
    {
        try
        {
            await _updateCar.WithAsync(request.ToCar(id));
            return NoContent();
        }
        catch (CarNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _deleteCar.ByAsync(id);
            return NoContent();
        }
        catch (CarNotFoundException)
        {
            return NotFound();
        }
    }
}
