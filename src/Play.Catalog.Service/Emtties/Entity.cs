namespace Play.Catalog.Service.Emtties;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; private set; }
}
