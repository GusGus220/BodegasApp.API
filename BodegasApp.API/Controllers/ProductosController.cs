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
    }
}