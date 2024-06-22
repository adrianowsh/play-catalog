namespace Play.Catalog.Service.Dtos;

public sealed record ItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    DateTimeOffset CreatesDate);

