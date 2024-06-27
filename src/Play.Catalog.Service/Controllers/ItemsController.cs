using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Emtties;
using Play.Common;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("api/items")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IRepository<Item> _itemsRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        _itemsRepository = itemsRepository;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = (await _itemsRepository.GetAllAsync())
            .Select(item => item.AsDto());
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);
        if (item is null) return NotFound();
        return Ok(item.AsDto());
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateItemDto createItemDto)
    {
        var item = Item.NewItem(
            createItemDto.Name,
            createItemDto.Description,
            createItemDto.Price,
            DateTimeOffset.UtcNow
        );
        await _itemsRepository.CreateAsync(item);

        var message = new CatalogItemCreated(
            item.Id,
            item.Name,
            item.Description
        );
        await _publishEndpoint.Publish(message);
        return CreatedAtAction(nameof(GetById), new { Id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, UpdatetemDto updatetemDto)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        if (existingItem is null) return NotFound();
        existingItem.SetName(updatetemDto.Name);
        existingItem.SetDescription(updatetemDto.Description);
        existingItem.SetPrice(updatetemDto.Price);
        await _itemsRepository.UpdateAsync(existingItem);

        var message = new CatalogItemUpdated(
            existingItem.Id,
            existingItem.Name,
            existingItem.Description
        );
        await _publishEndpoint.Publish(message);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        if (existingItem is null) return NotFound();
        await _itemsRepository.RemoveAsync(id);
        var message = new CatalogItemDeleted(existingItem.Id);
        await _publishEndpoint.Publish(message);
        return NoContent();
    }
}
