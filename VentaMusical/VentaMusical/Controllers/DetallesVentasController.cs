using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentaMusical.Controllers
{
    public class DetallesVentasController : Controller
    {
        // GET: DetallesVentas
        public ActionResult Index()
        {
            return View();
        }

        // GET: DetallesVentas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DetallesVentas/Create
        public ActionResult Create()
        {
            return View();
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
