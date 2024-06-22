using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Emtties;
using Play.Catalog.Service.Repositories;


namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("api/items")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository _itemsRepository;
    public ItemsController(IItemsRepository itemsRepository)
    {
        _itemsRepository = itemsRepository;
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
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingItem = await _itemsRepository.GetAsync(id);
        if (existingItem is null) return NotFound();
        await _itemsRepository.RemoveAsync(id);
        return NoContent();
    }
}
