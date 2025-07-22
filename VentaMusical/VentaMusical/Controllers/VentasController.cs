using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.Canciones;
using VentaMusical.Models.ViewModels.DetallesVenta;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaMusical.Models.ViewModels.Venta;
using Paragraph = iTextSharp.text.Paragraph;
using VentaModel = VentaMusical.Models.Venta.Venta;

namespace VentaMusical.Controllers
{
    public class VentasController : Controller
    {
        public class Context : DbContext
        private static List<CarritoItem> carrito = new List<CarritoItem>();
        
        public ActionResult Index()
        {
            var usuariosViewModel = db.Usuarios.Select(u => new Usuario
            {
                IDUsuario = u.ID,
                NumeroIdentificacion = u.NumeroIdentificacion,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Genero = u.Genero,
                Correo = u.CorreoElectronico,
                TipoTarjeta = u.TipoTarjeta,
                DineroDisponible = u.DineroDisponible,
                NumeroTarjeta = u.NumeroTarjeta,
                Contrasena = u.Contrasena,
                Perfil = u.Perfil
            }).ToList();

            var cancionesViewModel = db.Canciones.Where(c => c.CantidadDisponible > 0)
                .Select(c => new VentaMusical.Models.ViewModels.Canciones.Cancion
                {
                    CodigoCancion = c.CodigoCancion,
                    NombreCancion = c.NombreCancion,
                    Precio = c.Precio,
                    CantidadDisponible = c.CantidadDisponible,
                    NombreArtista = c.Albumes.Artistas.NombreArtistico,
                    NombreAlbum = c.Albumes.NombreAlbum,
                    ImagenAlbum = c.Albumes.Imagen
                }).ToList();

            var ventaFormulario = new VentaFormulario

            {
                Canciones = cancionesViewModel,
                Usuarios = usuariosViewModel,
                CarritoItems = carrito,
                EsAdmin = User.IsInRole("Administrador")
            };

            return View(ventaFormulario);
        }

        // POST: Agregar al carrito
        [HttpPost]
        public ActionResult AgregarAlCarrito(int codigoCancion, int cantidad = 1)
        {
            var cancion = db.Canciones.Find(codigoCancion);
            if (cancion == null || cancion.CantidadDisponible < cantidad)
            {
                return Json(new { success = false, message = "Canción no disponible o cantidad insuficiente" });
            }

            var itemCarrito = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
            if (itemCarrito != null)
            {
                if (itemCarrito.Cantidad + cantidad > cancion.CantidadDisponible)
                {
                    return Json(new { success = false, message = "Cantidad excede el inventario disponible" });
                }
                itemCarrito.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    CodigoCancion = codigoCancion,
                    NombreCancion = cancion.NombreCancion,
                    PrecioUnitario = cancion.Precio,
                    Cantidad = cantidad
                });
            }

            var subtotal = carrito.Sum(c => c.PrecioUnitario * c.Cantidad);
            return Json(new
            {
                success = true,
                subtotal = subtotal,
                totalItems = carrito.Sum(c => c.Cantidad)
            });
        }
        // POST: Remover del carrito
        [HttpPost]
        public ActionResult RemoverDelCarrito(int codigoCancion)
        {
            var item = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
            if (item != null)
            {
                carrito.Remove(item);
            }

            var subtotal = carrito.Sum(c => c.PrecioUnitario * c.Cantidad);
            return Json(new
            {
                success = true,
                subtotal = subtotal,
                totalItems = carrito.Sum(c => c.Cantidad)
            });
        }
        // GET: Obtener carrito actual
        public ActionResult ObtenerCarrito()
        {
            var subtotal = carrito.Sum(c => c.PrecioUnitario * c.Cantidad);
            var iva = subtotal * 0.13m;

            return Json(new
            {
                items = carrito,
                subtotal = subtotal,
                iva = iva,
                total = subtotal + iva
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Vaciar carrito
        [HttpPost]
        public ActionResult VaciarCarrito()
        {
            carrito.Clear();
            return Json(new { success = true });
        }

        // GET: Crear nuevo usuario (solo admin)
        [HttpGet]
        public ActionResult CrearUsuario()
        {
            if (!User.IsInRole("Administrador"))
            {
                return new HttpStatusCodeResult(403, "No autorizado");
            }

            return PartialView("_CrearUsuario");
        }
               
        [HttpPost]
        public ActionResult CrearUsuario(Usuario usuarioViewModel)
        {
            if (!User.IsInRole("Administrador"))
            {
                return Json(new { success = false, message = "No autorizado" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Map Usuario ViewModel to Usuarios Entity
                    var usuarioEntity = new Usuarios
                    {
                        NumeroIdentificacion = usuarioViewModel.NumeroIdentificacion,
                        Nombre = usuarioViewModel.Nombre,
                        Apellido = usuarioViewModel.Apellido,
                        Genero = usuarioViewModel.Genero,
                        CorreoElectronico = usuarioViewModel.Correo,
                        TipoTarjeta = usuarioViewModel.TipoTarjeta,
                        DineroDisponible = usuarioViewModel.DineroDisponible,
                        NumeroTarjeta = usuarioViewModel.NumeroTarjeta,
                        Contrasena = usuarioViewModel.Contrasena,
                        Perfil = usuarioViewModel.Perfil
                    };

                    db.Usuarios.Add(usuarioEntity);
                    db.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        usuario = new
                        {
                            ID = usuarioEntity.ID,
                            Nombre = usuarioEntity.Nombre + " " + usuarioEntity.Apellido
                        }
                    });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error al crear usuario: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Datos inválidos" });
        }
       
        [HttpPost]
        public ActionResult ProcesarFactura(int idUsuario, string tipoPago, string detallePago, string codigoTarjeta)
        {
            if (!carrito.Any())
            {
                return Json(new { success = false, message = "El carrito está vacío" });
            }

            // Validar disponibilidad de canciones
            foreach (var item in carrito)
            {
                var cancion = db.Canciones.Find(item.CodigoCancion);
                if (cancion == null || cancion.CantidadDisponible < item.Cantidad)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"La canción '{item.NombreCancion}' no tiene suficiente inventario disponible"
                    });
                }
            }

            // Validar datos de tarjeta si es necesario
            if (tipoPago == "Tarjeta" && (string.IsNullOrEmpty(codigoTarjeta) || codigoTarjeta.Length < 3))
            {
                return Json(new { success = false, message = "Código de tarjeta requerido" });
            }

            try
            {
                var usuario = db.Usuario.Find(idUsuario);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }

                decimal subtotal = carrito.Sum(c => c.PrecioUnitario * c.Cantidad);
                decimal iva = subtotal * 0.13m;
                decimal comision = tipoPago == "Tarjeta" ? subtotal * 0.02m : 0;
                decimal totalSinDescuento = subtotal + iva + comision;

                // Aplicar dinero disponible
                decimal montoAFavor = 0;
                decimal totalFinal = totalSinDescuento;

                if (usuario.DineroDisponible >= totalSinDescuento)
                {
                    usuario.DineroDisponible -= totalSinDescuento;
                    montoAFavor = totalSinDescuento;
                    totalFinal = 0;
                }
                else if (usuario.DineroDisponible > 0)
                {
                    totalFinal -= usuario.DineroDisponible;
                    montoAFavor = usuario.DineroDisponible;
                    usuario.DineroDisponible = 0;
                }

                
                var ventaEntity = new VentaModel
                {
                    IDUsuario = idUsuario,
                    FechaCompra = DateTime.Now,
                    Subtotal = subtotal,
                    IVA = iva,
                    Total = totalFinal,
                    TipoPago = tipoPago,
                    DetallePago = FormatearDetallePago(tipoPago, detallePago),
                    MontoAFavor = montoAFavor,
                    DetalleVentas = new List<DetalleVenta>()
                };

                // Agregar detalles de venta
                foreach (var item in carrito)
                {
                    ventaEntity.DetalleVentas.Add(new DetalleVenta
                    {
                        CodigoCancion = item.CodigoCancion,
                        PrecioUnitario = item.PrecioUnitario,
                        Cantidad = item.Cantidad,
                        Subtotal = item.PrecioUnitario * item.Cantidad
                    });
                }

                // Actualizar inventario
                foreach (var item in carrito)
                {
                    var cancion = db.Canciones.Find(item.CodigoCancion);
                    if (cancion != null)
                    {
                        cancion.CantidadDisponible -= item.Cantidad;
                    }
                }

                db.Ventas.Add(ventaEntity);
                db.SaveChanges();

                // Limpiar carrito
                carrito.Clear();

                // Generar y enviar factura
                GenerarYEnviarFactura(ventaEntity.NumeroFactura);

                return Json(new
                {
                    success = true,
                    message = "Venta procesada exitosamente",
                    numeroFactura = ventaEntity.NumeroFactura
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar la venta: " + ex.Message });
            }
        }

        private string FormatearDetallePago(string tipoPago, string detallePago)
        {
            if (tipoPago == "Tarjeta" && !string.IsNullOrEmpty(detallePago) && detallePago.Length >= 4)
            {
                return "****-****-****-" + detallePago.Substring(detallePago.Length - 4);
            }
            return detallePago;
        }

        private void GenerarYEnviarFactura(int numeroFactura)
        {
            try
            {
                var venta = db.Ventas.Include("DetalleVentas").Include("Usuario").FirstOrDefault(v => v.NumeroFactura == numeroFactura);
                if (venta == null) return;

                var usuario = db.Usuario.FirstOrDefault(u => u.ID == venta.IDUsuario);
                if (usuario == null) return;

                var detalles = venta.DetalleVenta.ToList();

                string ruta = Server.MapPath("~/Facturas/");
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);

                string archivoPDF = Path.Combine(ruta, $"Factura_{numeroFactura}.pdf");

                using (FileStream fs = new FileStream(archivoPDF, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var doc = new Document(PageSize.A4, 50, 50, 50, 50))
                    {
                        PdfWriter.GetInstance(doc, fs);
                        doc.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                        var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                        doc.Add(new Paragraph("FACTURA DE VENTA", titleFont) { Alignment = Element.ALIGN_CENTER });
                        doc.Add(new Paragraph(" "));

                        doc.Add(new Paragraph($"Factura No: {numeroFactura}", headerFont));
                        doc.Add(new Paragraph($"Fecha: {venta.FechaCompra:dd/MM/yyyy HH:mm}", normalFont));
                        doc.Add(new Paragraph(" "));

                        doc.Add(new Paragraph("DATOS DEL CLIENTE:", headerFont));
                        doc.Add(new Paragraph($"Nombre: {usuario.Nombre} {usuario.Apellido}", normalFont));
                        doc.Add(new Paragraph($"Correo: {usuario.CorreoElectronico}", normalFont));
                        doc.Add(new Paragraph($"Teléfono: {usuario.NumeroTarjeta}", normalFont));
                        doc.Add(new Paragraph(" "));

                        doc.Add(new Paragraph("DETALLE DE LA COMPRA:", headerFont));

                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 40f, 15f, 15f, 15f });

                        table.AddCell(new PdfPCell(new Phrase("Canción", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        table.AddCell(new PdfPCell(new Phrase("Cantidad", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        table.AddCell(new PdfPCell(new Phrase("Precio Unit.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        table.AddCell(new PdfPCell(new Phrase("Subtotal", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                        foreach (var detalle in detalles)
                        {
                            var cancion = db.Canciones.Find(detalle.CodigoCancion);
                            table.AddCell(new PdfPCell(new Phrase(cancion?.NombreCancion ?? "N/A", normalFont)));
                            table.AddCell(new PdfPCell(new Phrase("1", normalFont)));
                            table.AddCell(new PdfPCell(new Phrase($"₡{detalle.PrecioUnitario:N2}", normalFont)));
                            table.AddCell(new PdfPCell(new Phrase($"₡{detalle.PrecioUnitario:N2}", normalFont)));
                        }

                        doc.Add(table);
                        doc.Add(new Paragraph(" "));

                        doc.Add(new Paragraph("RESUMEN DE PAGOS:", headerFont));
                        doc.Add(new Paragraph($"Subtotal: ₡{venta.Subtotal:N2}", normalFont));
                        doc.Add(new Paragraph($"IVA (13%): ₡{venta.IVA:N2}", normalFont));

                        if (venta.TipoPago == "Tarjeta")
                        {
                            decimal comisión = venta.Subtotal * 0.02m; // Correct spelling of "comisión"
                            doc.Add(new Paragraph($"Comisión tarjeta (2%): ₡{comisión:N2}", normalFont));
                        }

                        if (venta.MontoAFavor > 0)
                        {
                            doc.Add(new Paragraph($"Monto cubierto con saldo disponible: ₡{venta.MontoAFavor:N2}", normalFont));
                        }

                        doc.Add(new Paragraph($"TOTAL A PAGAR: ₡{venta.Total:N2}", headerFont));
                        doc.Add(new Paragraph(" "));

                        doc.Add(new Paragraph($"Método de pago: {venta.TipoPago}", normalFont));
                        if (!string.IsNullOrEmpty(venta.DetallePago))
                        {
                            doc.Add(new Paragraph($"Detalle: {venta.DetallePago}", normalFont));
                        }

                        doc.Close();
                    }
                }

                EnviarFacturaPorCorreo(usuario.CorreoElectronico, numeroFactura, archivoPDF, usuario.Nombre);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error generando factura: {ex.Message}");
            }
        }

        private void EnviarFacturaPorCorreo(string correoDestino, int numeroFactura, string rutaArchivo, string nombreCliente)
        {
            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("ventamusical@tuempresa.com", "Venta Musical");
                correo.To.Add(correoDestino);
                correo.Subject = $"Factura No. {numeroFactura} - Venta Musical";
                correo.Body = $@"
                    Estimado/a {nombreCliente},

                    Adjuntamos la factura correspondiente a su compra realizada en nuestro sistema.

                    Número de factura: {numeroFactura}
                    Fecha: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}

                    Gracias por su preferencia.

                    Saludos cordiales,
                    Equipo Venta Musical
                ";

                correo.Attachments.Add(new Attachment(rutaArchivo));

                SmtpClient smtp = new SmtpClient("smtp.gmail.com"); // Cambiar por tu servidor SMTP
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("tu-email@gmail.com", "tu-password"); // Configurar
                smtp.EnableSsl = true;

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                // Log del error
                System.Diagnostics.Debug.WriteLine($"Error enviando correo: {ex.Message}");
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

        private readonly Context db = new Context();
    }
}
