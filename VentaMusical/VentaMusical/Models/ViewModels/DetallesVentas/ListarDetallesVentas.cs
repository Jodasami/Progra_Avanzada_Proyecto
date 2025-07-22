using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VentaMusical.Models.ViewModels.DetallesVenta
{
    public class ListarDetallesVentas : DetalleVenta
    {
        public class CrearDetalle
        {
            [Display(Name = "Factura No.")]
            public int NumeroFactura { get; set; }

            [Display(Name = "Nombre Canción")]
            public string NombreCancion { get; set; }

            [Display(Name = "Precio Unitario")]
            public decimal PrecioUnitario { get; set; }

            [Display(Name = "Fecha Compra")]
            public DateTime FechaCompra { get; set; }

            [Display(Name = "Usuario")]
            public string Usuario { get; set; }
        }
    }
}