using Bridge.Domain.Entities.Dog;
using Bridge.Domain.RepositoriesContext;

namespace Bridge.Domain.Repositories;

public interface IDogRepository
{
    Task<IEnumerable<Dog>> GetDogsAsync(GetDogsContext context, CancellationToken cancellationToken = default);
    Task<bool> IsNameUnique(string name,CancellationToken cancellationToken = default);
    void Add(Dog dog);
}
