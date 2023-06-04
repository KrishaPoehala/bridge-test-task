using Bridge.Infrastructure;
using Bridge.Application;
using Bridge.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Bridge.Middlewares;
using Bridge.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddGlobalRateLimiter();

builder.Services.AddSingleton<ExceptionMiddleware>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseMiddleware<ExceptionMiddleware>();
app.UseRateLimiter();
app.MapControllers();

InitializeDb(app);


app.Run();

static void InitializeDb(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<DogsContext>();
    context.Database.Migrate();
    var initializer = scope.ServiceProvider.GetRequiredService<DogsContextInitializer>();
    initializer.Initialize();
}