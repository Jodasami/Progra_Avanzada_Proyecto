using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Albumes;
using VentaMusical.Models.ViewModels.Generos;

namespace VentaMusical.Models.ViewModels.Canciones
{
    public class Cancion
    {
        public int CodigoCancion { get; set; }
        public string NombreCancion { get; set; }
        public decimal Precio { get; set; }
        public int CantidadDisponible { get; set; }
        public string NombreArtista { get; set; }
        public string NombreAlbum { get; set; }
        public string ImagenAlbum { get; set; }
        
        // Relaciones
        [ForeignKey("CodigoAlbum")]
        public virtual Album Album { get; set; }

        [ForeignKey("CodigoGenero")]
        public virtual Genero Genero { get; set; }
    }
}