using Bridge.Infrastructure;
using Bridge.Application;
using Bridge.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Bridge.Middlewares;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using Bridge.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: "default",
            factory: partition =>
            {
                var options = httpContext.RequestServices
                .GetRequiredService<IOptions<RateLimiterOptions>>().Value;

                return new FixedWindowRateLimiterOptions
                {
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    AutoReplenishment = options.AutoReplenishment,
                    PermitLimit = options.PermitLimit,
                    QueueLimit = options.QueueLimit,
                    Window = TimeSpan.FromSeconds(options.WindowInSeconds)
                };
            }));
});

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

void InitializeDb(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<DogsContext>();
    context.Database.Migrate();
    var initializer = scope.ServiceProvider.GetRequiredService<DogsContextInitializer>();
    initializer.Initialize();
}

app.Run();
