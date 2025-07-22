using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VentaMusical.Models.ViewModels.Canciones;
using VentaMusical.Models.ViewModels.DetallesVenta;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaMusical.Models.Venta;





namespace VentaMusical.Models.ViewModels.Venta
{
    public class VentaFormulario
    {
        public VentaModels Ventas { get; set; } 
        public List<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
        [Display(Name = "Listado de Canciones")]
        public List<Cancion> Canciones { get; set; }
        [Display(Name = "Usuarios disponibles")]
        public List<Usuario> Usuarios { get; set; }
        [Display(Name = "Elementos en el carrito")]
        public List<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();
        public bool EsAdmin { get; set; }
        public decimal TotalCarrito { get; set; }
        public decimal SubtotalCarrito { get; set; }
        public decimal IVACarrito { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo de pago")]
        public string TipoPagoSeleccionado { get; set; } = string.Empty;
        [StringLength(12, MinimumLength = 4, ErrorMessage = "El código debe tener entre 4 y 12 caracteres")]
        public string CodigoTarjeta { get; set; } = string.Empty;
        public bool UsarDineroDisponible { get; set; } = true;
    }
}