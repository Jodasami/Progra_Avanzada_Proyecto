using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Venta
{
    public class ProcesarVenta
    {
        [Required]
        [Display(Name = "Usuario")]
        public int IDUsuario { get; set; }

        [Required]
        [Display(Name = "Tipo de Pago")]
        public string TipoPago { get; set; }

        [Display(Name = "Detalle de Pago")]
        public string DetallePago { get; set; }

        [Display(Name = "Código de Tarjeta")]
        public string CodigoTarjeta { get; set; }

        public List<CarritoItem> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Comision { get; set; }
        public decimal Total { get; set; }
        public decimal DineroDisponible { get; set; }
        public decimal MontoAFavor { get; set; }
        public decimal TotalFinal { get; set; }
    }
}