using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentaMusical.Controllers
{
    public class GenerosController : Controller
    {
        // GET: Generos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Generos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Generos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generos/Create
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

        // GET: Generos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Generos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Generos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Generos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
