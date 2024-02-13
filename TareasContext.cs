using Microsoft.EntityFrameworkCore;
using primerproyecto.Models;

namespace primerproyecto;

//Hereda de la clase DBContext
public class TareasContext: DbContext
{
    public DbSet<Categoria> Categorias {get;set;}

    public DbSet<Tarea> Tareas {get; set;}

    //método base del constructor de EF
    public TareasContext(DbContextOptions<TareasContext> options): base(options){}

}