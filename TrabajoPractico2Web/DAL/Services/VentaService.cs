using Microsoft.EntityFrameworkCore;
using TrabajoPractico2Web.DAL.Data;
using TrabajoPractico2Web.Models;

namespace TrabajoPractico2Web.DAL.Services
{
    public class VentaService
    {
        private readonly AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Venta>> GetVentasAsync()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)                  // <-- Trae el nombre del cliente
                .Include(v => v.Detalles)                // <-- Trae los detalles
                    .ThenInclude(d => d.Producto)        // <-- Trae el nombre del producto
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();
        }

        public async Task<Venta?> GetVentaByIdAsync(int id)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddVentaAsync(Venta venta)
        {
            // 1. Validar el stock antes de hacer nada
            foreach (var detalle in venta.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    if (producto.Stock < detalle.Cantidad)
                    {
                        // Lanzamos un error si la cantidad supera el stock
                        throw new Exception($"Stock insuficiente para '{producto.Nombre}'. Stock disponible: {producto.Stock}");
                    }
                    // Descontamos el stock
                    producto.Stock -= detalle.Cantidad;
                }
            }

            // 2. Si todo está bien, guardamos la venta
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVentaAsync(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
        }
    }
}