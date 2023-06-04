using Bridge.Domain.Entities.Dog;

namespace Bridge.Infrastructure.Persistance;

public class DogsContextInitializer
{
    private readonly DogsContext _context;

    public DogsContextInitializer(DogsContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        if (_context.Dogs.Any())
        {
            return;
        }

        var dogs = new List<Dog>
        {
            new Dog {Id=Guid.NewGuid().ToString(), Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
            new Dog {Id=Guid.NewGuid().ToString(), Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 },
            new Dog {Id=Guid.NewGuid().ToString(), Name = "Max", Color = "brown", TailLength = 15, Weight = 18 },
            new Dog {Id=Guid.NewGuid().ToString(), Name = "Bella", Color = "white", TailLength = 10, Weight = 25 },
            // Add more dogs as needed
        };

        _context.Dogs.AddRange(dogs);
        _context.SaveChanges();
    }
}
