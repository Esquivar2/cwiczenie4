using Microsoft.AspNetCore.Mvc;
using APBD_Task_6.Models;
using Microsoft.AspNetCore.Mvc;
using Zadanie5.Services;
using APBD_Zadanie_6.Services;

namespace APBD_Zadanie_6.Controllers
{
    [ApiController]
    [Route("api/warehouse2")]
    public class WarehouseProcedureController : ControllerBase
    {
        private readonly IWarehouseProcedureService _warehouseProcedureService;

        public WarehouseProcedureController(IWarehouseProcedureService warehouseProcedureService)
        {
            _warehouseProcedureService = warehouseProcedureService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouseAsync(ProductWarehouse productWarehouse)
        {
            int idProductWarehouse = await _warehouseProcedureService.AddProductToWarehouseAsync(productWarehouse);
            return Ok(idProductWarehouse);
        }
    }
}
