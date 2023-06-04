using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Controllers;

public class ApiContollerBase:ControllerBase
{
    protected ISender Mediator => HttpContext.RequestServices.GetRequiredService<ISender>();
}
