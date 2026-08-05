using Microsoft.AspNetCore.Mvc;
using BodegasApp.API.Models;
using BodegasApp.API.Services;

namespace BodegasApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public TransaccionesController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        // Endpoint para procesar la venta y descontar stock / registrar en Kardex
        [HttpPost("venta")]
        public async Task<IActionResult> ProcesarVenta([FromBody] List<ItemCarrito> carrito)
        {
            var resultado = await _mongoDbService.ProcesarVentaAsync(carrito);
            if (resultado) return Ok(new { mensaje = "Venta exitosa" });
            return BadRequest(new { mensaje = "No se pudo procesar la venta" });
        }

        // Endpoint para obtener el historial del Kardex
        [HttpGet("historial")]
        public async Task<ActionResult<List<Movimiento>>> ObtenerHistorial()
        {
            var historial = await _mongoDbService.ObtenerHistorialAsync();
            return Ok(historial);
        }
    }
}
