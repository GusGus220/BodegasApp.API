namespace BodegasApp.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BodegasApp.API.Models;
    using BodegasApp.API.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public ProductosController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Producto>>> Get()
        {
            var productos = await _mongoDbService.GetAsync();
            return Ok(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest("El producto no puede estar vacío.");

            await _mongoDbService.CreateAsync(producto);
            return CreatedAtAction(nameof(Get), new { id = producto.Id }, producto);
        }

        // --- NUEVO: Endpoint para Actualizar (PUT) ---
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest("El producto no puede estar vacío.");

            var exito = await _mongoDbService.ActualizarProductoAsync(id, producto);
            if (exito) return Ok(new { mensaje = "Actualizado con éxito" });
            return NotFound(new { mensaje = "No se pudo actualizar el producto." });
        }

        // --- NUEVO: Endpoint para Eliminar (DELETE) ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var exito = await _mongoDbService.EliminarProductoAsync(id);
            if (exito) return Ok(new { mensaje = "Eliminado con éxito" });
            return NotFound(new { mensaje = "No se pudo encontrar o eliminar el producto." });
        }
    }
}
