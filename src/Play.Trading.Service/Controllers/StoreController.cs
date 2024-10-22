using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Service.Repositories;
using Play.Trading.Service.Dtos;
using Play.Trading.Service.Entities;

namespace Play.Trading.Service.Controllers
{
    [ApiController]
    [Route("store")]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly IRepository<CatalogItem> _catalogRepository;
        private readonly IRepository<ApplicationUser> _userRepository;

        private readonly IRepository<InventoryItem> _inventoryRepository;


        public StoreController(IRepository<CatalogItem> catalogRepository, IRepository<ApplicationUser> userRepository, IRepository<InventoryItem> inventoryRepository)
        {
            _catalogRepository = catalogRepository;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
        }


        [HttpGet]
        public async Task<ActionResult<StoreDto>> GetAsync()
        {
            var userId = User.FindFirstValue("sub");
            var catalogItems = await _catalogRepository.GetAllAsync();
            var inventoryItems = await _inventoryRepository.GetAllAsync(item =>
            item.UserId.ToString() == userId);

            var user = await _userRepository.GetAsync(Guid.Parse(userId));

            var storeDto = new StoreDto(
                catalogItems.Select(catalogItem =>
                new StoreItemDto(catalogItem.Id,
                catalogItem.Name,
                catalogItem.Description,
                catalogItem.Price,
                inventoryItems.FirstOrDefault(item => item.CatalogItemId == catalogItem.Id)?.Quantity ?? 0
                    )
                ),
                user?.Gil ?? 0
            );

            return Ok(storeDto);


        }
    }
}