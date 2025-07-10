using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentaMusical.Controllers
{
    public class VentasController : Controller
    {
        // GET: Ventas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ventas/Create
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
