using Bridge.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bridge.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection self)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        self.AddMediatR(options => options.RegisterServicesFromAssembly(currentAssembly));
        self.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        self.AddAutoMapper(currentAssembly);
        self.AddValidatorsFromAssembly(currentAssembly);
        return self;
    }
}
