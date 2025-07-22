using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Canciones;
using VentaMusical.Models.ViewModels.DetallesVenta;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaModel = VentaMusical.Models.Venta.Venta;


namespace VentaMusical.Models
{
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection") { }
        
        public DbSet<VentaModel> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cancion> Canciones { get; set; }

    }
}