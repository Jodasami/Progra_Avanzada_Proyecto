using System;
using System.Collections.Generic;
using VentaMusical.Models.ViewModels.Venta;


namespace VentaMusical.Models.ViewModels.Venta
{
    public class ReporteVentas
    {
        public List<VentaResumen> Ventas { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        // Estadísticas del periodo
        public decimal TotalVentas { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalComisiones { get; set; }
        public int CantidadVentas { get; set; }
        public int CantidadVentasReversadas { get; set; }
        public decimal PromedioVenta { get; set; }

        // Ventas por método de pago
       // public Dictionary<string, VentasPorMetodo> VentasPorTipoPago { get; set; }

        // Canciones más vendidas
        //public List<CancionMasVendida> TopCanciones { get; set; }

        // Usuarios con más compras
       // public List<UsuarioTopCompras> TopUsuarios { get; set; }
    }
}