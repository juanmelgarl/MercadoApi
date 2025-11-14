using Microsoft.EntityFrameworkCore;
using WebApplication4.Clases;

namespace WebApplication4.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrderItem> OrdenItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Orden → Items (1 → N)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Orden)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrdenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Producto → OrdenItems (1 → N)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Producto)
                .WithMany(p => p.OrdenItems)
                .HasForeignKey(oi => oi.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
