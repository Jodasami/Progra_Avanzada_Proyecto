
﻿using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Albumes;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace VentaMusical.Controllers
{
    public class AlbumesController : Controller
    {

        // 1. Instancia del contexto de la base de datos
        private VentaMusicalEntities db = new VentaMusicalEntities();

        // GET: Albumes (PANTALLA PRINCIPAL PARA CONSULTAR)
        public ActionResult Index(int? CodigoArtista)
        {
            // Carga la lista de artistas para el dropdown de filtro
            ViewBag.ListaDeArtistas = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico");

            // Carga la lista de álbumes
            var albumes = db.Albumes.Include(a => a.Artistas); // .Include() trae los datos del artista

            // Si se seleccionó un artista en el filtro, aplica el filtro
            if (CodigoArtista != null)
            {
                albumes = albumes.Where(a => a.CodigoArtista == CodigoArtista);
            }

            return View(albumes.ToList());
        }

        // GET: Albumes/Create (MUESTRA EL FORMULARIO PARA AGREGAR)
        public ActionResult Create()
        {
            // Carga la lista de artistas para el dropdown del formulario
            ViewBag.CodigoArtista = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico");
            return View();
        }

        // POST: Albumes/Create (GUARDA EL NUEVO ÁLBUM)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearAlbum viewModel, HttpPostedFileBase ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                string rutaImagen = "default.jpg"; // Imagen por defecto

                // Lógica para guardar la imagen subida
                if (ImagenArchivo != null && ImagenArchivo.ContentLength > 0)
                {
                    string nombreArchivo = System.IO.Path.GetFileName(ImagenArchivo.FileName);
                    string rutaFisica = System.IO.Path.Combine(Server.MapPath("~/Content/Imagenes/Albumes"), nombreArchivo);
                    ImagenArchivo.SaveAs(rutaFisica);
                    rutaImagen = nombreArchivo; // Guardas solo el nombre del archivo
                }

                var albumDB = new Albumes
                {
                    CodigoArtista = viewModel.CodigoArtista,
                    NombreAlbum = viewModel.NombreAlbum,
                    AnoLanzamiento = viewModel.AnoLanzamiento,
                    Imagen = rutaImagen
                };

                db.Albumes.Add(albumDB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoArtista = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico", viewModel.CodigoArtista);
            return View(viewModel);
        }

        // GET: Albumes/Edit/5 (MUESTRA EL FORMULARIO PARA EDITAR)
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albumes albumDB = db.Albumes.Find(id);
            if (albumDB == null)
            {
                return HttpNotFound();
            }

            // Mapea el modelo de la DB a tu ViewModel para enviarlo a la vista
            var viewModel = new EditarAlbum
            {
                CodigoAlbum = albumDB.CodigoAlbum,
                NombreAlbum = albumDB.NombreAlbum,
                AnoLanzamiento = albumDB.AnoLanzamiento,
                CodigoArtista = albumDB.CodigoArtista,
                ImagenExistente = albumDB.Imagen
            };

            ViewBag.CodigoArtista = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico", albumDB.CodigoArtista);
            return View(viewModel);
        }

        // POST: Albumes/Edit/5 (GUARDA LOS CAMBIOS DEL ÁLBUM)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarAlbum viewModel, HttpPostedFileBase ImagenArchivo)
        {
            if (ModelState.IsValid)
            {
                var albumEnDB = db.Albumes.Find(viewModel.CodigoAlbum);

                // Lógica para actualizar la imagen si se sube una nueva
                if (ImagenArchivo != null && ImagenArchivo.ContentLength > 0)
                {
                    string nombreArchivo = System.IO.Path.GetFileName(ImagenArchivo.FileName);
                    string rutaFisica = System.IO.Path.Combine(Server.MapPath("~/Content/Imagenes/Albumes"), nombreArchivo);
                    ImagenArchivo.SaveAs(rutaFisica);
                    albumEnDB.Imagen = nombreArchivo;
                }

                albumEnDB.NombreAlbum = viewModel.NombreAlbum;
                albumEnDB.AnoLanzamiento = viewModel.AnoLanzamiento;
                albumEnDB.CodigoArtista = viewModel.CodigoArtista;

                db.Entry(albumEnDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoArtista = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico", viewModel.CodigoArtista);
            return View(viewModel);
        }

        // GET: Albumes/Delete/5 (MUESTRA LA PÁGINA DE CONFIRMACIÓN)
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albumes albumes = db.Albumes.Find(id);
            if (albumes == null)
            {
                return HttpNotFound();
            }
            return View(albumes);
        }

        // POST: Albumes/Delete/5 (EJECUTA LA ELIMINACIÓN)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Albumes albumes = db.Albumes.Find(id);

            // Opcional pero recomendado: Borrar el archivo de imagen del servidor
            string rutaCompleta = Server.MapPath("~/Content/Imagenes/Albumes/" + albumes.Imagen);
            if (System.IO.File.Exists(rutaCompleta))
            {
                System.IO.File.Delete(rutaCompleta);
            }

            db.Albumes.Remove(albumes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Es una buena práctica liberar la conexión a la DB
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

