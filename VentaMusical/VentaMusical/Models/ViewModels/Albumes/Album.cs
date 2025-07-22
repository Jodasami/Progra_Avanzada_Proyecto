using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Artistas;
using VentaMusical.Models.ViewModels.Canciones;

namespace VentaMusical.Models.ViewModels.Albumes
{
	public class Album
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoAlbum { get; set; }

        [Required]
        public int CodigoArtista { get; set; }

        [StringLength(150)]
        public string NombreAlbum { get; set; }
        public string NombreArtistico { get; set; }

        public int AnoLanzamiento { get; set; }

        public string Imagen { get; set; }

        // Relación
        [ForeignKey("CodigoArtista")]
        public virtual Artista Artista { get; set; }

        public virtual ICollection<Cancion> Canciones { get; set; } = new List<Cancion>();
    }
}