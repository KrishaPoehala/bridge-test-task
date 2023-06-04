using AutoMapper;
using Bridge.Application.Common.Dtos;
using Bridge.Application.Dogs.Commands.CreateNewDog;
using Bridge.Application.Dogs.Queries.GetAllDogs;
using Bridge.Domain.Entities.Dog;
using Bridge.Domain.RepositoriesContext;

namespace Bridge.Application.Common.Mapping;

public class DogsMapping:Profile
{
    public DogsMapping()
    {
        CreateMap<Dog, DogDto>().ReverseMap();
        CreateMap<GetAllDogsQuery, GetDogsContext>();
        CreateMap<CreateNewDogCommand, Dog>();
    }
}
