
﻿using System.ComponentModel.DataAnnotations;

namespace VentaMusical.Models.ViewModels.Albumes
{
    public class EditarAlbum
    {
        
        public int CodigoAlbum { get; set; }

        [Required(ErrorMessage = "El nombre del álbum es obligatorio.")]
        [Display(Name = "Nombre del Álbum")]
        public string NombreAlbum { get; set; }

        [Required(ErrorMessage = "El año de lanzamiento es obligatorio.")]
        [Display(Name = "Año de Lanzamiento")]
        public int AnoLanzamiento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un artista.")]
        [Display(Name = "Artista")]
        public int CodigoArtista { get; set; }

        // Para mostrar la imagen actual en el formulario de edición
        public string ImagenExistente { get; set; }
    }

}