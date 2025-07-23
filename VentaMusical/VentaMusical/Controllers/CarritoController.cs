using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Carritos;

namespace VentaMusical.Controllers
{
    public class CarritoController : Controller
    {
        private VentaMusicalEntities db = new VentaMusicalEntities();

        private List<CarritoItem> ObtenerCarrito()
        {
            var carrito = Session["Carrito"] as List<CarritoItem>;
            if (carrito == null)
            {
                carrito = new List<CarritoItem>();
                Session["Carrito"] = carrito;
            }
            return carrito;
        }

        public ActionResult Index()
        {
            var carrito = ObtenerCarrito();
            return View(carrito);
        }

        public ActionResult Agregar(int codigoCancion, int cantidad)
        {
            var cancion = db.Canciones.Find(codigoCancion);
            if (cancion == null || cantidad > cancion.CantidadDisponible)
                return RedirectToAction("Index");

            var carrito = ObtenerCarrito();

            var existente = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    CodigoCancion = cancion.CodigoCancion,
                    NombreCancion = cancion.NombreCancion,
                    PrecioUnitario = cancion.Precio,
                    Cantidad = cantidad
                });
            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int codigoCancion)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
            if (item != null)
            {
                carrito.Remove(item);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Facturar(int idUsuario, string tipoPago, string codigoTarjeta)
        {
            var carrito = ObtenerCarrito();
            if (!carrito.Any()) return RedirectToAction("Index");

            var usuario = db.Usuarios.Find(idUsuario);
            if (usuario == null) return RedirectToAction("Index");

            decimal subtotal = carrito.Sum(c => c.SubTotal);
            decimal iva = Math.Round(subtotal * 0.13m, 2);
            decimal recargo = tipoPago.Contains("tarjeta") ? Math.Round(subtotal * 0.02m, 2) : 0;
            decimal montoAFavor = usuario.DineroDisponible;
            decimal totalFinal = subtotal + iva + recargo;

            decimal montoUsado = montoAFavor >= totalFinal ? totalFinal : montoAFavor;
            decimal montoRestante = montoAFavor - montoUsado;
            decimal totalConCredito = totalFinal - montoUsado;

            var venta = new Ventas
            {
                IDUsuario = idUsuario,
                FechaCompra = DateTime.Now,
                TotalSinIVA = subtotal,
                IVA = iva,
                RecargoTarjeta = recargo,
                TotalFinal = totalFinal,
                TipoPago = tipoPago,
                CodigoTarjeta = codigoTarjeta,
                MontoCreditoUsado = montoUsado,
                MontoRestante = montoRestante,
                Estado = "Facturada",
                EnviadaPorCorreo = false
            };

            db.Ventas.Add(venta);
            db.SaveChanges();

            foreach (var item in carrito)
            {
                var detalle = new DetalleVenta
                {
                    NumeroFactura = venta.NumeroFactura,
                    CodigoCancion = item.CodigoCancion,
                    NombreCancion = item.NombreCancion,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad,
                    Subtotal = item.SubTotal
                };

                db.DetalleVenta.Add(detalle);

                var cancion = db.Canciones.Find(item.CodigoCancion);
                cancion.CantidadDisponible -= item.Cantidad;
            }

            usuario.DineroDisponible = montoRestante;
            db.SaveChanges();

            Session["Carrito"] = null;

            return RedirectToAction("Details", "Ventas", new { id = venta.NumeroFactura });
        }

        public ActionResult Cancelar()
        {
            Session["Carrito"] = null;
            return RedirectToAction("Index");
        }
    }
}
