using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VentaMusical.Models.ViewModels.DetallesVenta;
using VentaMusical.Models.ViewModels.Usuarios;



namespace VentaMusical.Models.Venta
{
	public class Venta
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumeroFactura { get; set; }

        [Required]
        [Display(Name = "ID Usuario")]
        public int IDUsuario { get; set; }

        [Required]
        [Display(Name = "Fecha de Compra")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Total")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "Subtotal")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [Required]
        [Display(Name = "IVA")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal IVA { get; set; }

        [Required]
        [Display(Name = "Tipo de Pago")]
        [StringLength(25)]
        public string TipoPago { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Detalle de Pago")]
        [StringLength(100)]
        public string DetallePago { get; set; } = string.Empty;

        [Display(Name = "Monto a Favor")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoAFavor { get; set; } = 0;
        [Display(Name = "Estado de la Venta")]
        [StringLength(20)]
        public string EstadoVenta { get; set; } = "Procesada";

        [Display(Name = "Fecha de Reversión")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaReversion { get; set; }

        [Display(Name = "Motivo de Reversión")]
        [StringLength(150)]
        public string MotivoReversion { get; set; }

        [NotMapped]
        public string NombreUsuario => $"{Usuario?.Nombre} {Usuario?.Apellido}";

        [Display(Name = "Fecha de Reversa")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaRecervar { get; set; }

        // Propiedades de navegación
        [ForeignKey("IDUsuario")]
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<DetalleVenta> DetalleVentas { get; set; }  // Colección de detalles de venta asociados a esta venta

        [NotMapped]
        public decimal ComisionTarjeta => TipoPago == "Tarjeta" ? Subtotal * 0.02m : 0;   // 2% de comisión para pagos con tarjeta
        public decimal TotalConIVA => Subtotal + IVA; // Total con IVA incluido
        public string FechaCompraFormat => FechaCompra.ToString("dd/MM/yyyy HH:mm"); // Formato de fecha para mostrar en la vista
        public bool UsóSaldo => MontoAFavor > 0; // Indica si se utilizó saldo a favor en la compra
        public bool FueReversada => FechaRecervar != null; // Indica si la venta fue reversada

        [NotMapped]
        public bool EsReversible  // Indica si la venta puede ser reversada
        {
            get
            {
                return FechaRecervar == null &&
                                   (DateTime.Now - FechaCompra).TotalHours <= 24;
            }
        }

        [NotMapped]
        public bool EstaReversada  // Indica si la venta ya fue reversada
        {
            get
            {
                return FechaRecervar != null;
            }
        }  
    }
}
