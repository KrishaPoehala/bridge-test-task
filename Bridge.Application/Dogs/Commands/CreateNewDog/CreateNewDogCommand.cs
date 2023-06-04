using AutoMapper;
using Bridge.Application.Common.Interfaces;
using Bridge.Domain.Entities.Dog;
using Bridge.Domain.Repositories;
using MediatR;

namespace Bridge.Application.Dogs.Commands.CreateNewDog;

public record CreateNewDogCommand(string Name, string Color, int TailLength, int Weight)
    : IRequest;

public class CreateNewDogCommandHandler : IRequestHandler<CreateNewDogCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDogRepository _repository;

    public CreateNewDogCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IDogRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task Handle(CreateNewDogCommand request, CancellationToken cancellationToken)
    {
        var ifExists = await _repository.IsNameUnique(request.Name, cancellationToken);
        if (ifExists)
        {
            throw new InvalidDataException($"Dog with name {request.Name} alread exists");
        }

        var entity = _mapper.Map<Dog>(request);
        entity.Id = Guid.NewGuid().ToString();
        _repository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
