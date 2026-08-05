namespace BodegasApp.API.Services
{
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using BodegasApp.API.Models;

    public class MongoDbService
    {
        private readonly IMongoCollection<Producto> _productosCollection;
        // Aquí declaramos la colección
        private readonly IMongoCollection<Movimiento> _movimientosCollection;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);

            _productosCollection = mongoDatabase.GetCollection<Producto>("productos");
            // Aquí la inicializamos dentro del constructor
            _movimientosCollection = mongoDatabase.GetCollection<Movimiento>("movimientos");
        }

        // --- MÉTODOS DE PRODUCTOS ---
        public async Task<List<Producto>> GetAsync() =>
            await _productosCollection.Find(_ => true).ToListAsync();

        public async Task<bool> ActualizarProductoAsync(string id, Producto productoActualizado)
        {
            var resultado = await _productosCollection.ReplaceOneAsync(p => p.Id == id, productoActualizado);
            return resultado.IsAcknowledged && resultado.ModifiedCount > 0;
        }

        public async Task<bool> EliminarProductoAsync(string id)
        {
            var resultado = await _productosCollection.DeleteOneAsync(p => p.Id == id);
            return resultado.IsAcknowledged && resultado.DeletedCount > 0;
        }

        public async Task CreateAsync(Producto nuevoProducto)
        {
            // 1. Guardar el producto
            await _productosCollection.InsertOneAsync(nuevoProducto);

            // 2. Registrar automáticamente el Ingreso en el Kardex
            var movimiento = new Movimiento
            {
                Tipo = "📥 INGRESO",
                ProductoNombre = nuevoProducto.Nombre,
                Cantidad = nuevoProducto.Stock,
                Total = 0 // El ingreso de stock no genera ganancia directa en caja
            };
            await RegistrarMovimientoAsync(movimiento);
        }

        // --- MÉTODOS DE TRANSACCIONES Y KARDEX ---
        public async Task<List<Movimiento>> ObtenerHistorialAsync() =>
            await _movimientosCollection.Find(_ => true).SortByDescending(m => m.Fecha).ToListAsync();

        public async Task RegistrarMovimientoAsync(Movimiento movimiento) =>
            await _movimientosCollection.InsertOneAsync(movimiento);

        public async Task<bool> ProcesarVentaAsync(List<ItemCarrito> carrito)
        {
            try
            {
                foreach (var item in carrito)
                {
                    // 1. Buscar el producto
                    var producto = await _productosCollection.Find(p => p.Codigo == item.Codigo).FirstOrDefaultAsync();
                    if (producto == null || producto.Stock < item.Cantidad) continue;

                    // 2. Descontar Stock
                    var update = Builders<Producto>.Update.Inc(p => p.Stock, -item.Cantidad);
                    await _productosCollection.UpdateOneAsync(p => p.Codigo == item.Codigo, update);

                    // 3. Registrar Movimiento (Kardex)
                    var movimiento = new Movimiento
                    {
                        Tipo = "🛒 SALIDA",
                        ProductoNombre = item.Nombre,
                        Cantidad = item.Cantidad,
                        Total = item.Cantidad * item.Precio
                    };
                    await RegistrarMovimientoAsync(movimiento);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
