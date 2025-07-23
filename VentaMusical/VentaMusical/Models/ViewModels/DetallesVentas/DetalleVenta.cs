using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Canciones;

namespace VentaMusical.Models.ViewModels.DetallesVentas
{
	public class DetalleVenta
	{
        public int IDDetalle { get; set; }
        public int NumeroFactura { get; set; }
        public int CodigoCancion { get; set; }
        public string NombreCancion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }

        // Navegación opcional
        public VentaMusical.Models.Ventas Venta { get; set; }
        public Cancion Cancion { get; set; }
    }
}