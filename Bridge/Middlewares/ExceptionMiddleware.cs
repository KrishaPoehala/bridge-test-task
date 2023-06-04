using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Bridge.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
            await next(context);
		}
		catch (Exception ex)
		{

            var response = context.Response;
            response.StatusCode = ex switch
            {
                NullReferenceException => (int)System.Net.HttpStatusCode.NotFound,
                _ => (int)System.Net.HttpStatusCode.BadRequest,
            };

            var result = System.Text.Json.JsonSerializer.Serialize(new { message = ex.Message });
            response.ContentType = "application/json";
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(result));
        }
    }
}
