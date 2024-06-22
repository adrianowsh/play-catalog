using MongoDB.Driver;
using Play.Catalog.Service.Emtties;

namespace Play.Catalog.Service.Repositories;

public sealed class ItemsRepository
{
    private const string collectionName = "items";
    private readonly IMongoCollection<Item> dbCollection;
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public ItemsRepository()
    {
        var mongoCLient = new MongoClient("mongodb://localhost:27017");
        var database = mongoCLient.GetDatabase("Catalog");
        dbCollection = database.GetCollection<Item>(collectionName);
    }

    public async Task<IReadOnlyCollection<Item>> GetAllAsync()
    {
        var result = await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        return result;
    }

    public async Task<Item> GetAsync(Guid id)
    {
        var filter = filterBuilder.Eq(e => e.Id, id);
        var result = await dbCollection.FindAsync(filter);
        return await result.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Item entity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(entity));
        await dbCollection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(Item entity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(entity));
        var filter = filterBuilder.Eq(e => e.Id, entity.Id);
        await dbCollection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = filterBuilder.Eq(e => e.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
}
