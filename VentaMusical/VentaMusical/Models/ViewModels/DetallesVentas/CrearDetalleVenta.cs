using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VentaMusical.Models.ViewModels.DetallesVenta
{
	public class CrearDetalleVenta
	{
        [Display(Name = "Código Canción")]
        public int CodigoCancion { get; set; }

        [Display(Name = "Nombre Canción")]
        public string NombreCancion { get; set; }

        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
    }
}