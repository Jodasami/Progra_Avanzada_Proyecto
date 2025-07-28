using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models.ViewModels.Ventas;
using VentaMusical.Models;

namespace VentaMusical.Controllers
{
    public class VentasController : Controller
    {
        private VentaMusicalEntities db = new VentaMusicalEntities();
        // GET: Ventas
        public ActionResult Index(string busquedaFactura, string estado, string tipoPago)
        {
            try
            {
                var ventasFiltradas = db.Ventas.AsQueryable();

                if (!string.IsNullOrEmpty(busquedaFactura))
                {
                    // Si es numérico, filtrá por NumeroFactura
                    if (int.TryParse(busquedaFactura, out int numero))
                    {
                        ventasFiltradas = ventasFiltradas.Where(v => v.NumeroFactura == numero);
                    }
                    else
                    {
                        // Si no, buscá por nombre del usuario
                        ventasFiltradas = ventasFiltradas.Where(v => v.Usuarios.Nombre.Contains(busquedaFactura));
                    }
                }
                // Filtrar por estado de la compra
                if (!string.IsNullOrEmpty(estado))
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.Estado == estado);
                }

                // 💳 Filtrar por tipo de pago
                if (!string.IsNullOrEmpty(tipoPago))
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.TipoPago == tipoPago);
                }
                var viewModel = ventasFiltradas.Select(v => new ListarVentas

                {
                    NumeroFactura = v.NumeroFactura,
                    IDUsuario = v.IDUsuario,
                    FechaCompra = v.FechaCompra,
                    TotalSinIVA = v.TotalSinIVA,
                    IVA = v.IVA,
                    RecargoTarjeta = v.RecargoTarjeta,
                    TotalFinal = v.TotalFinal,
                    TipoPago = v.TipoPago,
                    CodigoTarjeta = v.CodigoTarjeta,
                    Estado = v.Estado
                }).ToList();

                return View(viewModel);
            }
            catch
            {
                return View(new List<ListarVentas>());
            }
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var venta = db.Ventas.Find(id);
                if (venta == null)
                    return HttpNotFound();

                var detalle = new Venta
                {
                    NumeroFactura = venta.NumeroFactura,
                    IDUsuario = venta.IDUsuario,
                    FechaCompra = venta.FechaCompra,
                    TotalSinIVA = venta.TotalSinIVA,
                    IVA = venta.IVA,
                    RecargoTarjeta = venta.RecargoTarjeta,
                    TotalFinal = venta.TotalFinal,
                    TipoPago = venta.TipoPago,
                    CodigoTarjeta = venta.CodigoTarjeta,
                    MontoCreditoUsado = venta.MontoCreditoUsado ?? 0,
                    MontoRestante = venta.MontoRestante ?? 0,
                    Estado = venta.Estado,
                    EnviadaPorCorreo = venta.EnviadaPorCorreo ?? false,
                };
                return View(detalle);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Ventas", "Details"));
            }
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre");
            return View();
        }

        // POST: Ventas/Create
        [HttpPost]
        public ActionResult Create(Venta venta)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre", venta.IDUsuario);
                    return View(venta);
                }

                //Cálculo automático Iva
                decimal recargo = venta.RecargoTarjeta;
                venta.IVA = Math.Round(venta.TotalSinIVA * 0.13m, 2);
                venta.TotalFinal = Math.Round(venta.TotalSinIVA + venta.IVA + recargo, 2);

                //Construcción de entidad

                var nuevaVenta = new Ventas
                {
                    IDUsuario = venta.IDUsuario,
                    FechaCompra = DateTime.Now,
                    TotalSinIVA = venta.TotalSinIVA,
                    IVA = venta.IVA,
                    RecargoTarjeta = venta.RecargoTarjeta,
                    TotalFinal = venta.TotalFinal,
                    TipoPago = venta.TipoPago,
                    CodigoTarjeta = venta.CodigoTarjeta,
                    MontoCreditoUsado = venta.MontoCreditoUsado,
                    MontoRestante = venta.MontoRestante,
                    Estado = venta.Estado,
                    EnviadaPorCorreo = venta.EnviadaPorCorreo
                };
                //Guarda la venta en la base de datos
                db.Ventas.Add(nuevaVenta);
                db.SaveChanges();

                // Confirmación número generado de factura
                int numeroFactura = nuevaVenta.NumeroFactura;

                ViewBag.MensajeProceso = "Venta registrada exitosamente";
                ViewBag.ValorMensaje = 1;
                ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre");

                return View(new Venta()); // Limpia el formulario
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al registrar la venta: " + ex.Message;
                ViewBag.ValorMensaje = 0;
                ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre", venta.IDUsuario);
                return View(venta);
            }
        }

        // GET: Ventas/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var venta = db.Ventas.Find(id);
                if (venta == null)
                    return HttpNotFound();

                var vm = new Venta
                {
                    NumeroFactura = venta.NumeroFactura,
                    IDUsuario = venta.IDUsuario,
                    FechaCompra = venta.FechaCompra,
                    TotalSinIVA = venta.TotalSinIVA,
                    IVA = venta.IVA,
                    RecargoTarjeta = venta.RecargoTarjeta,
                    TotalFinal = venta.TotalFinal,
                    TipoPago = venta.TipoPago,
                    CodigoTarjeta = venta.CodigoTarjeta,
                    MontoCreditoUsado = venta.MontoCreditoUsado ?? 0,
                    MontoRestante = venta.MontoRestante ?? 0,
                    Estado = venta.Estado,
                    EnviadaPorCorreo = venta.EnviadaPorCorreo ?? false
                };

                ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre", venta.IDUsuario);
                return View(vm);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Ventas", "Edit"));
            }
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        public ActionResult Edit(Venta venta)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre", venta.IDUsuario);
                return View(venta);
            }

            var ventaActual = db.Ventas.Find(venta.NumeroFactura);

            if (ventaActual == null)
            {
                ViewBag.MensajeProceso = "La venta no existe o fue eliminada.";
                ViewBag.ValorMensaje = 0;
                ViewBag.IDUsuario = new SelectList(db.Usuarios, "ID", "Nombre", venta.IDUsuario);
                return View(venta);
            }

            ventaActual.IDUsuario = venta.IDUsuario;
            ventaActual.FechaCompra = venta.FechaCompra;
            ventaActual.TotalSinIVA = venta.TotalSinIVA;
            ventaActual.IVA = venta.IVA;
            ventaActual.RecargoTarjeta = venta.RecargoTarjeta;
            ventaActual.TotalFinal = venta.TotalFinal;
            ventaActual.TipoPago = venta.TipoPago;
            ventaActual.CodigoTarjeta = venta.CodigoTarjeta;
            ventaActual.MontoCreditoUsado = venta.MontoCreditoUsado;
            ventaActual.MontoRestante = venta.MontoRestante;
            ventaActual.Estado = venta.Estado;
            ventaActual.EnviadaPorCorreo = venta.EnviadaPorCorreo;


            db.Entry(ventaActual).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.MensajeProceso = "Venta actualizada correctamente";
            ViewBag.ValorMensaje = 1;
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public ActionResult Delete(int id)
        {
            var venta = db.Ventas.Find(id);
            if (venta == null)
                return HttpNotFound();

            var vm = new Venta
            {
                NumeroFactura = venta.NumeroFactura,
                FechaCompra = venta.FechaCompra,
                TotalFinal = venta.TotalFinal,
                TipoPago = venta.TipoPago,
                Estado = venta.Estado
            };

            return View(vm);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var venta = db.Ventas.Find(id);
            if (venta != null)
            {
                db.Ventas.Remove(venta);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
