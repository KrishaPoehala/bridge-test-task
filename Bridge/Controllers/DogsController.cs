using Bridge.Application.Dogs.Commands.CreateNewDog;
using Bridge.Application.Dogs.Queries.GetAllDogs;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Controllers;

[ApiController]
public class DogsController : ApiContollerBase
{
    [HttpGet]
    [Route("ping")]
    public string Ping()
    {
        return "Dogs house service. Version 1.0.1";
    }

    [HttpGet]
    [Route("dog")]
    public async Task<IActionResult> GetDogs([FromQuery] int pageNumber, [FromQuery] int pageSize,
        [FromQuery] string attribute = "name", [FromQuery] string order = "desc")
    {
        var query = new GetAllDogsQuery(
             attribute,
             order,
             pageNumber,
             pageSize);

        var dogs = await Mediator.Send(query);

        return Ok(dogs);
    }

    [HttpPost]
    [Route("dog")]
    public async Task<IActionResult> Create(CreateNewDogCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
