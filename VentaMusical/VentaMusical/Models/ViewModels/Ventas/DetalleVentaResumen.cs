using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace VentaMusical.Models.ViewModels.Venta
{
    public class DetalleVentaResumen
    {
        public string NombreCancion { get; set; }
        public string NombreAlbum { get; set; }
        public string NombreArtista { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
        
    }
}