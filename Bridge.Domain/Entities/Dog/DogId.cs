namespace Bridge.Domain.Entities.Dog;

public record struct DogId
{
    public string Value { get; init; }
    public DogId(string value)
    {
        Value = value;
    }

    public static implicit operator DogId(string value) => new(value);

    public static implicit operator string(DogId dogId) => dogId.Value;

    public override string ToString() => Value;
}