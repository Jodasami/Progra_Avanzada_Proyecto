using System;
using System.Collections.Generic;




namespace VentaMusical.Models.ViewModels.Venta
{
    public class VentaResumen
    {
        public int NumeroFactura { get; set; }
        public DateTime FechaCompra { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoUsuario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public string TipoPago { get; set; }
        public string DetallePago { get; set; }
        public decimal MontoAFavor { get; set; }
        public string EstadoVenta { get; set; }
        public DateTime? FechaReversion { get; set; }
        public int CantidadCanciones { get; set; }
        public bool PuedeReversar { get; set; }
        public List<DetalleVentaResumen> Detalles { get; set; }
    }
}