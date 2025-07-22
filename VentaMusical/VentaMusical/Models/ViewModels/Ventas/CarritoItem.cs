using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Venta
{
    public class CarritoItem
    {
        public int CodigoCancion { get; set; }
        public string NombreCancion { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal Subtotal => PrecioUnitario * Cantidad;
        public string NombreArtista { get; set; } = string.Empty;
        public string NombreAlbum { get; set; } = string.Empty;
        public string ImagenAlbum { get; set; } = string.Empty;
    }
}