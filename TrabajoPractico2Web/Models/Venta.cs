using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabajoPractico2Web.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }

        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required]
        public decimal Total { get; set; }

        // Propiedad de navegación: Una venta tiene muchos detalles (productos)
        public List<DetalleVenta> Detalles { get; set; } = new();
    }
}
