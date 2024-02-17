using Microsoft.EntityFrameworkCore;
using primerproyecto.Models;

namespace primerproyecto;

//Inherits from DBContext Class
public class TareasContext: DbContext
{
    public DbSet<Categoria> Categorias {get;set;}

    public DbSet<Tarea> Tareas {get; set;}

    //método base del constructor de EF
    public TareasContext(DbContextOptions<TareasContext> options): base(options){}


//Overriding method OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder){

        //List of "Categorías" to populate DB
        List<Categoria> categoriasInit = new List<Categoria>();
        categoriasInit.Add(new Categoria(){CategoriaId = Guid.Parse("055199d2-c55a-4033-b816-519f49000dee"),Nombre = "Actividades Pendientes",Peso = 20});
        categoriasInit.Add(new Categoria(){CategoriaId = Guid.Parse("055199d2-c55a-4033-b816-519f49000d00"),Nombre = "Actividades Personales",Peso = 50}); 

    //Building restrictions for Categories
        modelBuilder.Entity<Categoria>(categoria=>
        {
            //Expression to indicate that I want Categoria as a Table
            categoria.ToTable("Categoria");
            //ID Table
            categoria.HasKey(p=>p.CategoriaId);
            categoria.Property(p=>p.Nombre).IsRequired().HasMaxLength(150);
            categoria.Property(p=>p.Descripcion).IsRequired(false);
            categoria.Property(p=>p.Peso);
            categoria.HasData(categoriasInit);
        });

        //List of "Tareas" to populate DB
        List<Tarea> tareasInit = new List<Tarea>();
        tareasInit.Add(new Tarea(){TareaId = Guid.Parse("bb1cee1a-db90-49de-8eed-06587faacf63"), CategoriaId = Guid.Parse("055199d2-c55a-4033-b816-519f49000dee"), PrioridadTarea = Prioridad.Media, Titulo = "Pago de servicios publicos", FechaCreacion = DateTime.Now});
        tareasInit.Add(new Tarea(){TareaId = Guid.Parse("bb1cee1a-db90-49de-8eed-06587faacf64"), CategoriaId = Guid.Parse("055199d2-c55a-4033-b816-519f49000d00"), PrioridadTarea = Prioridad.Baja, Titulo = "Terminar de leer mi libro", FechaCreacion = DateTime.Now});

        modelBuilder.Entity<Tarea>(tarea=>
        {
            tarea.ToTable("Tarea");
            tarea.HasKey(p=> p.TareaId);
            tarea.HasOne(p=> p.Categoria).WithMany(p=> p.Tareas).HasForeignKey(p=> p.CategoriaId);
            tarea.Property(p=> p.Titulo).IsRequired().HasMaxLength(200);
            tarea.Property(p=> p.Descripcion).IsRequired(false);
            tarea.Property(p=> p.PrioridadTarea);
            tarea.Property(p=> p.FechaCreacion);
            tarea.Ignore(p=> p.Resumen);
            tarea.HasData(tareasInit);

        });
    }

}