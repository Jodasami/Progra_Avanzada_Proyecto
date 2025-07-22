using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaMusical.Models.ViewModels.Venta;


namespace VentaMusical.Models.ViewModels.DetallesVenta
{
    public class HistorialCompras
    {
        public List<VentaResumen> Ventas { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public bool EsAdmin { get; set; }
        public bool EsContabilidad { get; set; }

        [Display(Name = "Filtrar por Usuario")]
        public int? FiltroUsuario { get; set; }

        [Display(Name = "Fecha Desde")]
        [DataType(DataType.Date)]
        public DateTime FechaDesde { get; set; }

        [Display(Name = "Fecha Hasta")]
        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; }

        // Estadísticas
        public decimal TotalVentas { get; set; }
        public int CantidadVentas { get; set; }
        public decimal PromedioVenta { get; set; }
       
    }
}