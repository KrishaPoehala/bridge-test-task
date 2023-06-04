using Bridge.Domain.Entities.Dog;
using Bridge.Domain.Repositories;
using Bridge.Domain.RepositoriesContext;
using Bridge.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Bridge.Infrastructure.Repositories;

//to cover this class with tests it's better to use integrational testing(because this class uses DbContext 
//which is hard to mock)
//but the test task didn't require me to write those,so I'm gonna leave it as is
public class DogRepository : IDogRepository
{
    private readonly DogsContext _context;

    public DogRepository(DogsContext context)
    {
        _context = context;
    }

    public Task<IEnumerable<Dog>> GetDogsAsync(GetDogsContext context, CancellationToken cancellationToken = default)
    {
        var query = _context.Dogs.AsQueryable();
        query = OrderIfAttributeExists(context, query);

        query = context.PageNumber == 0 ? query :
            query.Skip(context.PageNumber * context.PageSize);

        query = context.PageSize == 0 ? query :
            query.Take(context.PageSize);

        return Task.FromResult(query.AsEnumerable());
    }

    private static readonly IEnumerable<string?> _chachedDogProperties;
    static DogRepository()
    {
        _chachedDogProperties = typeof(Dog).GetProperties().Select(x => x.Name.ToLower());
    }

    private static IQueryable<Dog> OrderIfAttributeExists(GetDogsContext context, IQueryable<Dog> query)
    {
        if (string.IsNullOrEmpty(context.Attribute))
        {
            return query;
        }

        if (!_chachedDogProperties.Contains(context.Attribute.ToLower()))
        {
            throw new InvalidDataException($"Class dog does not contain property with name {context.Attribute}");
        }

        var orderExpression = $"{context.Attribute} {GetOrder(context)}";
        return query.OrderBy(orderExpression);
    }

    private static string GetOrder(GetDogsContext context)
    {
        return context.Order?.ToLower() == "desc" ? "descending" : "ascending";
    }

    public async Task<bool> IsNameUnique(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Dogs.AnyAsync(x => x.Name == name, cancellationToken);
    }

    public void Add(Dog dog) => _context.Dogs.Add(dog);
}
