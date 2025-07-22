using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Mail;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.Venta;
using VentaMusical.Models.ViewModels.DetallesVenta;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaMusical.Models.ViewModels.Venta;
using DetalleVenta = VentaMusical.Models.ListarDetalleVenta_Result;


namespace VentaMusical.Controllers
{
    public class DetallesVentasController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            var esAdmin = User.IsInRole("Administrador");
            var esContabilidad = User.IsInRole("Contabilidad");

            var ventas = db.Ventas
                .Include("Usuario")
                .Include("DetalleVentas.Cancion.Album.Artista")
                .Select(v => new VentaResumen
                {
                    NumeroFactura = v.NumeroFactura,
                    FechaCompra = v.FechaCompra,
                    NombreUsuario = v.Usuario.Nombre + " " + v.Usuario.Apellido,
                    CorreoUsuario = v.Usuario.Correo,
                    Subtotal = v.Subtotal,
                    IVA = v.IVA,
                    Total = v.Total,
                    TipoPago = v.TipoPago,
                    DetallePago = v.DetallePago,
                    MontoAFavor = v.MontoAFavor,
                    EstadoVenta = v.EstadoVenta,
                    FechaReversion = v.FechaReversion,
                    CantidadCanciones = v.DetalleVentas.Count,
                    PuedeReversar = esContabilidad && (DateTime.Now - v.FechaCompra).TotalHours <= 24,
                    Detalles = v.DetalleVentas.Select(d => new DetalleVentaResumen
                    {
                        NombreCancion = d.Cancion.NombreCancion,
                        NombreAlbum = d.Cancion.Album.NombreAlbum,
                        NombreArtista = d.Cancion.Album.Artista.NombreArtistico,
                        PrecioUnitario = d.PrecioUnitario,
                        Cantidad = d.Cantidad,
                        Subtotal = d.Subtotal
                    }).ToList()
                }).ToList();

            if (!esAdmin && !esContabilidad)
            {
                var usuarioActual = ObtenerUsuarioActual();
                if (usuarioActual != null)
                {
                    var nombreCompleto = usuarioActual.Nombre + " " + usuarioActual.Apellido;
                    ventas = ventas.Where(v => v.NombreUsuario == nombreCompleto).ToList();
                }
            }

            var usuarios = esAdmin || esContabilidad
                ? db.Usuarios.Select(u => new Usuario
                {
                    IDUsuario = u.IDUsuario,
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Genero = u.Genero,
                    Correo = u.Correo,
                    TipoTarjeta = u.TipoTarjeta,
                    DineroDisponible = u.DineroDisponible,
                    NumeroTarjeta = u.NumeroTarjeta,
                    Perfil = u.Perfil
                }).ToList()
                : new List<Usuario>();

            var viewModel = new HistorialCompras
            {
                Ventas = ventas.OrderByDescending(v => v.FechaCompra).ToList(),
                Usuarios = usuarios,
                EsAdmin = esAdmin,
                EsContabilidad = esContabilidad,
                FiltroUsuario = null,
                FechaDesde = DateTime.Now.AddMonths(-1),
                FechaHasta = DateTime.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult FiltrarHistorial(int? filtroUsuario, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var esAdmin = User.IsInRole("Administrador");
            var esContabilidad = User.IsInRole("Contabilidad");

            IQueryable<Venta> ventas = db.Ventas.Include("Usuario").Include("DetalleVentas");

            if (!esAdmin && !esContabilidad)
            {
                var usuarioActual = ObtenerUsuarioActual();
                if (usuarioActual != null)
                {
                    ventas = ventas.Where(v => v.IDUsuario == usuarioActual.IDUsuario);
                }
            }
            else
            {
                if (filtroUsuario.HasValue)
                    ventas = ventas.Where(v => v.IDUsuario == filtroUsuario.Value);

                if (esContabilidad)
                {
                    if (fechaDesde.HasValue)
                        ventas = ventas.Where(v => v.FechaCompra >= fechaDesde.Value);
                    if (fechaHasta.HasValue)
                        ventas = ventas.Where(v => v.FechaCompra < fechaHasta.Value.AddDays(1));
                }
            }

            var resumen = ventas
                .OrderByDescending(v => v.FechaCompra)
                .Select(v => new VentaResumen
                {
                    NumeroFactura = v.NumeroFactura,
                    FechaCompra = v.FechaCompra,
                    EstadoVenta = v.EstadoVenta,
                    Total = v.Total,
                    TipoPago = v.TipoPago,
                    CorreoUsuario = v.Usuario.Correo
                }).ToList();

            var usuarios = esAdmin || esContabilidad
                ? db.Usuarios.Select(u => new Usuario
                {
                    IDUsuario = u.IDUsuario,
                    NumeroIdentificacion = u.NumeroIdentificacion,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Genero = u.Genero,
                    Correo = u.Correo,
                    TipoTarjeta = u.TipoTarjeta,
                    DineroDisponible = u.DineroDisponible,
                    NumeroTarjeta = u.NumeroTarjeta,
                    Perfil = u.Perfil
                }).ToList()
                : new List<Usuario>();

            var viewModel = new HistorialCompras
            {
                Ventas = resumen,
                Usuarios = usuarios,
                EsAdmin = esAdmin,
                EsContabilidad = esContabilidad,
                FiltroUsuario = filtroUsuario,
                FechaDesde = fechaDesde ?? DateTime.Now.AddMonths(-1),
                FechaHasta = fechaHasta ?? DateTime.Now
            };

            return View("Index", viewModel);
        }

        private List<DetalleVentaResumen> AgruparDetallesPorCancion(List<DetalleVenta> detalles)
        {
            var agrupados = detalles
                .GroupBy(d => d.CodigoCancion)
                .Select(grupo => new DetalleVentaResumen
                {
                    NombreCancion = grupo.First().Cancion?.NombreCancion ?? grupo.First().NombreCancion,
                    NombreAlbum = grupo.First().Cancion?.Album?.NombreAlbum ?? "Álbum no disponible",
                    NombreArtista = grupo.First().Cancion?.Album?.Artista?.NombreArtistico ?? "Artista no disponible",
                    PrecioUnitario = grupo.First().PrecioUnitario,
                    Cantidad = grupo.Sum(d => d.Cantidad),
                    Subtotal = grupo.Sum(d => d.Subtotal)
                })
                .OrderBy(d => d.NombreCancion)
                .ToList();

            return agrupados;
        }

        public ActionResult DetalleVenta(int id)
        {
            var venta = db.Ventas.Include("Usuario").Include("DetalleVentas.Cancion.Album.Artista")
                .FirstOrDefault(v => v.NumeroFactura == id);

            if (venta == null) return HttpNotFound();

            var esAdmin = User.IsInRole("Administrador");
            var esContabilidad = User.IsInRole("Contabilidad");
            var usuarioActual = ObtenerUsuarioActual();

            if (!esAdmin && !esContabilidad && (usuarioActual == null || venta.IDUsuario != usuarioActual.IDUsuario))
                return new HttpStatusCodeResult(403, "No autorizado");

            var puedeReversar = esContabilidad && (DateTime.Now - venta.FechaCompra).TotalHours <= 24;

            var viewModel = new DetalleVentaViewModel
            {
                Venta = venta,
                DetallesAgrupados = AgruparDetallesPorCancion(venta.DetalleVentas.ToList()),
                PuedeReversar = puedeReversar,
                EsContabilidad = esContabilidad,
                EsAdmin = esAdmin,
                TotalSinIVA = venta.Subtotal,
                TotalConIVA = venta.Total,
                ComisionTarjeta = venta.TipoPago == "Tarjeta" ? venta.Subtotal * 0.02m : 0
            };

            return View(viewModel);
        }
        private Usuario ObtenerUsuarioActual()
        {
            var correoActual = User.Identity.Name;

            // Busca el usuario en base al correo
            var usuarioBD = db.Usuarios.FirstOrDefault(u => u.Correo == correoActual);

            if (usuarioBD == null)
                return null;

            return new Usuario
            {
                IDUsuario = usuarioBD.IDUsuario,
                NumeroIdentificacion = usuarioBD.NumeroIdentificacion,
                Nombre = usuarioBD.Nombre,
                Apellido = usuarioBD.Apellido,
                Genero = usuarioBD.Genero,
                Correo = usuarioBD.Correo,
                TipoTarjeta = usuarioBD.TipoTarjeta,
                DineroDisponible = usuarioBD.DineroDisponible,
                NumeroTarjeta = usuarioBD.NumeroTarjeta,
                Perfil = usuarioBD.Perfil
            };
        }

            [HttpPost]
        public ActionResult ReversarVenta(int numeroFactura)
        {
            if (!User.IsInRole("Contabilidad"))
                return Json(new { success = false, message = "No autorizado" });

            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    var venta = db.Ventas.Include("Usuario").Include("DetalleVentas")
                        .FirstOrDefault(v => v.NumeroFactura == numeroFactura);

                    if (venta == null)
                        return Json(new { success = false, message = "Venta no encontrada" });

                    if ((DateTime.Now - venta.FechaCompra).TotalHours > 24)
                        return Json(new { success = false, message = "La venta no puede ser reversada." });

                    foreach (var detalle in venta.DetalleVentas)
                    {
                        var cancion = db.Canciones.Find(detalle.CodigoCancion);
                        if (cancion != null) cancion.CantidadDisponible += detalle.Cantidad;
                    }

                    decimal montoDevolucion = venta.Total + venta.MontoAFavor;
                    venta.Usuario.DineroDisponible += montoDevolucion;

                    venta.EstadoVenta = "Reversada";
                    venta.FechaReversion = DateTime.Now;

                    db.SaveChanges();
                    transaccion.Commit();

                    EnviarNotificacionReversion(venta, montoDevolucion);

                    return Json(new { success = true, message = "Venta reversada", montoDevuelto = montoDevolucion });
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return Json(new { success = false, message = "Error: " + ex.Message });
                }
            }
        }

        private void EnviarNotificacionReversion(Venta venta, decimal monto)
        {
            try
            {
                string smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"];
                string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

                var correo = new MailMessage
                {
                    From = new MailAddress("ventamusical@tuempresa.com", "Venta Musical"),
                    Subject = $"Reversión de Venta - Factura No. {venta.NumeroFactura}",
                    Body = $@"Estimado/a {venta.Usuario.Nombre},

                            Le informamos que su compra ha sido reversada con éxito.

                            Factura No: {venta.NumeroFactura}
                            Fecha original: {venta.FechaCompra:dd/MM/yyyy HH:mm}
                            Fecha de reversión: {DateTime.Now:dd/MM/yyyy HH:mm}
                            Monto devuelto: ₡{monto:N2}

                            Detalles de la compra:
                            {string.Join("\n", venta.DetalleVentas.Select(d => $"{d.Cancion.NombreCancion} - {d.Cantidad} unidades"))}

                            El monto ha sido acreditado a su saldo disponible en el sistema.

                            Gracias por su preferencia,
                            Equipo de Venta Musical",
                                                IsBodyHtml = false
                };

                correo.To.Add(venta.Usuario.Correo);

                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword),
                    EnableSsl = true
                };

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error enviando notificación: {ex.Message}");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);

        }
    }     
}