namespace Bridge.Domain.RepositoriesContext;

public record GetDogsContext(string? Attribute, string? Order, int PageNumber, int PageSize);
