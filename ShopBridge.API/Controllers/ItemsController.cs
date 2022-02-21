using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBridge.Data.Interfaces;
using ShopBridge.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class ItemsController : ControllerBase
    {
        private ILogger<ItemsController> _logger;
        private readonly IMapper _mapper;
        private IItemRepository _itemRepository;
        public ItemsController(ILogger<ItemsController> logger, IMapper mapper, IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{name}", Name = "GetItem")]
        public async Task<ActionResult<ItemModel>> GetAsync(string name)
        {
            try
            {
                var item = await _itemRepository.GetItemAsync(name);
                if (item == null)
                {
                    _logger.LogWarning($"Item with given name: {name} does not exists in the store.");
                    return NotFound();
                }
                var itemInfo = _mapper.Map<ItemModel>(item);
                return new OkObjectResult(itemInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching item information: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error fetching item information");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync(string searchText = null, int pageSize = 50, int page = 1)
        {
            try
            {
                if (pageSize <= 0)
                    pageSize = 50;
                if (page < 1)
                    page = 1;

                var item = await _itemRepository.GetAllItemAsync(searchText);

                var count = item.Count;

                var paginatedItems = item
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Select(u => _mapper.Map<ItemModel>(u))
                            .ToList();

                var paginatedList = new PaginatedList<ItemModel>(paginatedItems, count, page, pageSize);
                return new OkObjectResult(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching all item information: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error fetching all items information");
            }
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PostAsync([FromBody] ItemModel item)
        {
            try
            {

                var exists = await _itemRepository.IsItemExistsAsync(item.Name);

                if (exists)
                {
                    var err = $"Item with given name already exists.";
                    _logger.LogWarning(err);
                    return new ConflictResult();
                }
                var newItem = _mapper.Map<Data.Entities.ItemEntity>(item);
                await _itemRepository.AddItemAsync(newItem);
                await _itemRepository.SaveAsync();
                return CreatedAtRoute("GetItem", new { name = newItem.Name }, newItem.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating item: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error creating item");
            }
        }

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateAsync([FromBody] ItemModel item)
        {
            try
            {
                var name = item.Name;
                var existingItem = await _itemRepository.GetItemAsync(name);

                if (existingItem == null)
                {
                    _logger.LogWarning($" Item name :{name} does not exists in the store.");
                    return NotFound();
                }

                var entityToUpdate = _mapper.Map(item, existingItem);
                await _itemRepository.UpdateItemAsync(entityToUpdate);
                return Ok(new { message = "Item updated successfully" });
            }



            catch (Exception ex)
            {
                _logger.LogError($"Error deleting item : {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error deleting the item");
            }
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            try
            {
                var user = await _itemRepository.GetItemAsync(name);

                if (user == null)
                {
                    _logger.LogWarning($"Item with given name: {name} does not exists in the store.");
                    return NotFound();
                }
                // item can delete their own account and admins can delete any account
                var deleted = await _itemRepository.DeleteItemAsync(name);
                if (!deleted)
                {
                    var error = $"Failed to delete item {name}";
                    _logger.LogError(error);
                    return new BadRequestObjectResult(error);
                }
                await _itemRepository.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting item : {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error deleting item");
            }
        }
    }
}