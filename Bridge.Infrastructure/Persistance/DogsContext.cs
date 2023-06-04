using Bridge.Application.Common.Interfaces;
using Bridge.Domain.Entities.Dog;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Persistance;

public class DogsContext: DbContext, IUnitOfWork
{
    public DbSet<Dog> Dogs { get; set; }

    public DogsContext(DbContextOptions<DogsContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(builder);
    }
}
