using FluentValidation;

namespace Bridge.Application.Dogs.Commands.CreateNewDog;

public class CreateDogCommandValidator:AbstractValidator<CreateNewDogCommand>
{
    public CreateDogCommandValidator()
    {
        RuleFor(x => x.Weight).GreaterThan(0).LessThan(100);
        RuleFor(x => x.TailLength).GreaterThan(0).LessThan(100);
        RuleFor(x => x.Color).NotEmpty().NotNull().MinimumLength(0).MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(0).MaximumLength(100);
    }
}
