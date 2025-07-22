using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VentaMusical.Models.ViewModels.Venta;



namespace VentaMusical.Models.ViewModels.Ventas
{
	public class ListarVentas : Ventas
    {
        public ListarVentas()
        {
            DetalleVentaResumen = new List<DetalleVentaResumen>();
        }
        public List<DetalleVentaResumen> DetalleVentaResumen { get; set; }
    }
    public class Ventas
    {
        [Display(Name = "Factura")]
        public int NumeroFactura { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Fecha de Compra")]
        public DateTime FechaCompra { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "IVA")]
        public decimal IVA { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Display(Name = "Tipo de Pago")]
        public string TipoPago { get; set; }

        [Display(Name = "Monto a Favor")]
        public decimal MontoAFavor { get; set; }
    }
}