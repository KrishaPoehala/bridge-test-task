using FluentValidation;

namespace Bridge.Application.Dogs.Queries.GetAllDogs;

public class GetAllDogsQueryValidator:AbstractValidator<GetAllDogsQuery>
{
	public GetAllDogsQueryValidator()
	{
		RuleFor(x => x.Order).NotNull().NotEmpty().Length(0, 100);
		RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(0);
		RuleFor(x => x.Attribute).NotEmpty().NotNull().Length(0, 100);
		RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0);
	}
}