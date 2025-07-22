using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VentaMusical.Models.ViewModels.Venta
{
	public class CrearVenta
	{
        [Required]
        [Display(Name = "ID Usuario")]
        public int IDUsuario { get; set; }

        [Display(Name = "Fecha de la Compra")]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }

        [Display(Name = "IVA")]
        public decimal IVA { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Required]
        [Display(Name = "Tipo de Pago")]
        public string TipoPago { get; set; }

        [Display(Name = "Detalle del Pago")]
        public string DetallePago { get; set; }

        [Display(Name = "Monto a Favor")]
        public decimal MontoAFavor { get; set; }

        [Display(Name = "Canciones Seleccionadas")]
        public List<int> CancionesSeleccionadas { get; set; }

        // Para mostrar nombre de canciones seleccionadas (opcional)
        public List<string> NombresCanciones { get; set; }

        public IEnumerable<SelectListItem> ListaUsuarios { get; set; }
        public IEnumerable<SelectListItem> ListaCanciones { get; set; }
    }
}