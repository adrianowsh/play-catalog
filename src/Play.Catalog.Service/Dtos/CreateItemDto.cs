using System.ComponentModel.DataAnnotations;

namespace Play.Catalog.Service.Dtos;

public record CreateItemDto
{
  [Required]
  public string Name { get; init; }
  public string Description { get; init; }
  [Range(0, 1000)]
  public decimal Price { get; init; }
  public DateTimeOffset CreateDate { get; init; }

  public static CreateItemDto NewItem(string name, string Description, decimal price, DateTimeOffset createdDate)
  {
    return new CreateItemDto
    {
      Name = name,
      Description = Description,
      Price = price,
      CreateDate = createdDate
    };
  }
}

