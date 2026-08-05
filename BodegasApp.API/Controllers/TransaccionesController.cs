namespace BodegasApp.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BodegasApp.API.Models;
    using BodegasApp.API.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionesController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public TransaccionesController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet("historial")]
        public async Task<ActionResult<List<Movimiento>>> GetHistorial()
        {
            var historial = await _mongoDbService.ObtenerHistorialAsync();
            return Ok(historial);
        }

        [HttpPost("cobrar")]
        public async Task<IActionResult> Cobrar([FromBody] List<ItemCarrito> carrito)
        {
            if (carrito == null || !carrito.Any())
                return BadRequest("El carrito está vacío.");

            var exito = await _mongoDbService.ProcesarVentaAsync(carrito);

            if (exito) return Ok(new { status = "ok", mensaje = "Venta procesada con éxito" });

            return StatusCode(500, "Error al procesar la venta");
        }
    }
}