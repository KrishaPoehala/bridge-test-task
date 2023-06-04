using Bridge.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace Bridge.Extentions;

public static class IServiceCollectionExtentions
{
    public static IServiceCollection AddGlobalRateLimiter(this IServiceCollection self)
    {
        self.AddRateLimiter(options =>
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

        return self;
    }
   
}
