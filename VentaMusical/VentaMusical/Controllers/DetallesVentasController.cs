using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models;

namespace VentaMusical.Controllers
{
    public class DetallesVentasController : Controller
    {
        private VentaMusicalEntities db = new VentaMusicalEntities();
        // GET: DetallesVentas
        public ActionResult Index()
        {
            var detalles = db.DetalleVenta
            .Include("Ventas")
            .Include("Canciones")
            .ToList();
            return View(detalles);
        }

        // GET: DetallesVentas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DetallesVentas/Create
        public ActionResult Create([Bind(Include = "NumeroFactura,CodigoCancion,Cantidad")] DetalleVenta detalle)
        {
            var cancion = db.Canciones.Find(detalle.CodigoCancion);

            if (cancion != null)
            {
                detalle.NombreCancion = cancion.NombreCancion;
                detalle.PrecioUnitario = cancion.Precio;
                detalle.Subtotal = detalle.Cantidad * cancion.Precio;
            }

            if (ModelState.IsValid)
            {
                db.DetalleVenta.Add(detalle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NumeroFactura = new SelectList(db.Ventas, "NumeroFactura", "NumeroFactura", detalle.NumeroFactura);
            ViewBag.CodigoCancion = new SelectList(db.Canciones, "CodigoCancion", "NombreCancion", detalle.CodigoCancion);
            return View(detalle);
        }

        // POST: DetallesVentas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
