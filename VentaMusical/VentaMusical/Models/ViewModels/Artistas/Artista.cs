using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Albumes;

namespace VentaMusical.Models.ViewModels.Artistas
{
	public class Artista
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodigoArtista { get; set; }

        [Required]
        public string NombreArtistico { get; set; }

        public string NombreReal { get; set; }

        public string Nacionalidad { get; set; }

        public string Foto { get; set; }

        public string LinkBiografia { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public virtual ICollection<Album> Albumes { get; set; } = new List<Album>();
    }
}