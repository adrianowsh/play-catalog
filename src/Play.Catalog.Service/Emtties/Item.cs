using Play.Common;

namespace Play.Catalog.Service.Emtties;

public sealed class Item : Entity
{
    private Item(string name, string description, decimal price, DateTimeOffset createDate)
    {
        Name = name;
        Description = description;
        Price = price;
        CreateDate = createDate;
    }

    private Item() { }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTimeOffset CreateDate { get; private set; }

    public static Item NewItem(string name, string description, decimal price, DateTimeOffset createdDate)
        => new(name, description, price, createdDate);

    public void SetName(string newName) => Name = newName;
    public void SetDescription(string newDescription) => Description = newDescription;
    public void SetPrice(decimal newPrice) => Price = newPrice;
}
