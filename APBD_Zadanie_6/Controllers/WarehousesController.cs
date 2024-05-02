using APBD_Task_6.Models;
using Microsoft.AspNetCore.Mvc;
using Zadanie5.Services;

namespace Zadanie5.Controllers
{
    [Route("api/warehouse")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehousesController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductWarehouse product)
        {
            int idProduct = await _warehouseService.AddProduct(product);
            return Ok(idProduct);
        }
    }
}
