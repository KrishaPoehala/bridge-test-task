using AutoMapper;
using Bridge.Application.Common.Dtos;
using Bridge.Domain.Repositories;
using Bridge.Domain.RepositoriesContext;
using MediatR;

namespace Bridge.Application.Dogs.Queries.GetAllDogs;

//I don't see a point to write unit tests for this method since this handler is pretty straight forward
//(consists of only 3 lines of code and all of them would have to be mocked)
public record GetAllDogsQuery(string Attribute, string Order, int PageNumber, int PageSize)
    : IRequest<IEnumerable<DogDto>>;

public class GetAllDogsQueryHandler : IRequestHandler<GetAllDogsQuery, IEnumerable<DogDto>>
{
    private readonly IDogRepository _dogsRepository;
    private readonly IMapper _mapper;

    public GetAllDogsQueryHandler(IDogRepository dogsRepository, IMapper mapper)
    {
        _dogsRepository = dogsRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DogDto>> Handle(GetAllDogsQuery request, CancellationToken cancellationToken)
    {
        var context = _mapper.Map<GetDogsContext>(request);
        var dogs = await _dogsRepository.GetDogsAsync(context, cancellationToken);
        var result = _mapper.Map<IEnumerable<DogDto>>(dogs.AsEnumerable()).ToList();
        return result;
    }
}
