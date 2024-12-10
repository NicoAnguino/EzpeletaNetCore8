using EzpeletaNetCore8.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EzpeletaNetCore8.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TipoEjercicio> TipoEjercicios { get; set; }
    public DbSet<EjercicioFisico> EjerciciosFisicos { get; set; }
    public DbSet<Lugar> Lugares { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Deportista> Deportistas { get; set; }

}
