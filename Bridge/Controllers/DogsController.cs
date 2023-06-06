using Bridge.Application.Dogs.Commands.CreateNewDog;
using Bridge.Application.Dogs.Queries.GetAllDogs;
using Bridge.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bridge.Controllers;

[ApiController]
public class DogsController : ApiContollerBase
{
    private readonly PingMessageOptions _options;

    public DogsController(IOptions<PingMessageOptions> options)
    {
        _options = options.Value;
    }

    [HttpGet]
    [Route("ping")]
    public string Ping()
    {
        return _options.Message;
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
