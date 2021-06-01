using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBridge.Api.Data;
using ShopBridge.Api.Dtos;
using ShopBridge.Api.Models;

namespace ShopBridge.Api.Controllers
{
    [Route("/Api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryRepo repository, IMapper mapper, ILogger<InventoryController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemReadDto>>> GetAllInventoriesAsync()
        {
            _logger.LogInformation($"Request received to get all inventories. Method : GetAllInventoriesAsync");
            var inventoryItems = await _repository.GetAllInventoriesAsync();
            _logger.LogInformation($"Request completed to get all inventories. Method : GetAllInventoriesAsync");
            return Ok(_mapper.Map<IEnumerable<InventoryItemReadDto>>(inventoryItems));
        }

        [HttpGet("{id}", Name = "GetInventoryItemByIdAsync")]
        public async Task<ActionResult<InventoryItemReadDto>> GetInventoryItemByIdAsync(int id)
        {
            _logger.LogInformation($"Request received to get inventory item. Item id - {id}. Method : GetInventoryItemByIdAsync");
            var inventoryItem = await _repository.GetInventoryItemByIdAsync(id);
            if (inventoryItem is null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Request completed to get inventory item. Item id - {id}. Method : GetInventoryItemByIdAsync");
            return Ok(_mapper.Map<InventoryItemReadDto>(inventoryItem));
        }

        [HttpPost]
        public async Task<ActionResult> CreateInventoryItemAsync(InventoryItemCreateDto inventoryItemCreateDto)
        {
            _logger.LogInformation($"Request received to add inventory item. Method : CreateInventoryItemAsync");
            var inventoryItemModel = _mapper.Map<InventoryItem>(inventoryItemCreateDto);
            await _repository.CreateInventoryItemAsync(inventoryItemModel);
            var InventoryItemReadDto = _mapper.Map<InventoryItemReadDto>(inventoryItemModel);
            _logger.LogInformation($"Request completed to add inventory item. Item Id - {inventoryItemModel.Id}.  Method : CreateInventoryItemAsync");
            return CreatedAtRoute(nameof(GetInventoryItemByIdAsync), new { id = inventoryItemModel.Id }, InventoryItemReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInventoryItemAsync(int id, InventoryItemUpdateDto inventoryItemUpdateDto)
        {
            _logger.LogInformation($"Request received to update inventory item. Item Id - {id}. Method : UpdateInventoryItemAsync");
            var inventoryItemModelFromRepo = await _repository.GetInventoryItemByIdAsync(id);
            if (inventoryItemModelFromRepo is null)
            {
                return NotFound();
            }
            _mapper.Map(inventoryItemUpdateDto, inventoryItemModelFromRepo);
            await _repository.UpdateInventoryItemAsync(inventoryItemModelFromRepo);
            _logger.LogInformation($"Request completed to update inventory item. Item Id - {id}.  Method : UpdateInventoryItemAsync");
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateInventoryItemAsync(int id, JsonPatchDocument<InventoryItemUpdateDto> patchDocument)
        {
            _logger.LogInformation($"Request received to partially update inventory item. Item Id - {id}. Method : PartialUpdateInventoryItemAsync");
            var inventoryItemModelFromRepo = await _repository.GetInventoryItemByIdAsync(id);
            if (inventoryItemModelFromRepo is null)
            {
                return NotFound();
            }
            var inventoryItemToPatch = _mapper.Map<InventoryItemUpdateDto>(inventoryItemModelFromRepo);
            patchDocument.ApplyTo(inventoryItemToPatch, ModelState);
            if (!TryValidateModel(inventoryItemToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(inventoryItemToPatch, inventoryItemModelFromRepo);
            await _repository.UpdateInventoryItemAsync(inventoryItemModelFromRepo);
            _logger.LogInformation($"Request completed to partially update inventory item. Item Id - {id}.  Method : PartialUpdateInventoryItemAsync");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventoryItemAsync(int id)
        {
            _logger.LogInformation($"Request received to delete inventory item. Item Id - {id}. Method : DeleteInventoryItemAsync");
            var inventoryItemModelFromRepo = await _repository.GetInventoryItemByIdAsync(id);
            if (inventoryItemModelFromRepo is null)
            {
                return NotFound();
            }
            await _repository.DeleteInventoryItemAsync(inventoryItemModelFromRepo);
            _logger.LogInformation($"Request completed to delete inventory item. Item Id - {id}.  Method : DeleteInventoryItemAsync");
            return NoContent();
        }
    }
}