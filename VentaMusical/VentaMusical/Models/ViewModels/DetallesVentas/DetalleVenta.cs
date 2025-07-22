using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VentaMusical.Models.ViewModels.Canciones;



namespace VentaMusical.Models.ViewModels.DetallesVenta
{
	public class DetalleVenta
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDDetalle { get; set; }

        [Required]
        [Display(Name = "Número de Factura")]
        public int NumeroFactura { get; set; }

        [Required]
        [Display(Name = "Código de Canción")]
        public int CodigoCancion { get; set; }

        [Required]
        [Display(Name = "Nombre de Canción")]
        [StringLength(150)]
        public string NombreCancion { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Precio Unitario")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; } = 1;

        [Required]        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        // Propiedades de navegación      
        
        [ForeignKey("CodigoCancion")]
        public virtual Cancion Cancion { get; set; }
    }
}