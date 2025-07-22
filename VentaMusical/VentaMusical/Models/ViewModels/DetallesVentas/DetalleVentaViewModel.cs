using System;
using System.Collections.Generic;
using VentaMusical.Models.ViewModels.Venta;

namespace VentaMusical.Models.ViewModels.DetallesVenta
{
    public class DetalleVentaViewModel
    {
        public VentaMusical.Models.Venta.Venta Venta { get; set; }
        public List<DetalleVentaResumen> DetallesAgrupados { get; set; }
        public bool PuedeReversar { get; set; }
        public bool EsContabilidad { get; set; }
        public bool EsAdmin { get; set; }
        public decimal TotalSinIVA { get; set; }
        public decimal TotalConIVA { get; set; }
        public decimal ComisionTarjeta { get; set; }
        
    }
}