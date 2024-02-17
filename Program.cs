using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using primerproyecto;
using primerproyecto.Models;


var builder = WebApplication.CreateBuilder(args);

//Using this to create a DB in memory
//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));

//Configuration to connect with mysql
builder.Services.AddMySQLServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", ([FromServices] TareasContext dbContext) => 
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());
});

//Obtains all the "Tareas" with low priority
app.MapGet("/api/tareaslowpriority", async ([FromServices] TareasContext dbContext) =>
{
    return Results.Ok(dbContext.Tareas.Include(p=>p.Categoria).Where(p=>p.PrioridadTarea == primerproyecto.Models.Prioridad.Baja));
});

app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) =>
{
    return Results.Ok(dbContext.Tareas.Include(p=>p.Categoria));
});

//Store a new "Tarea"
app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) =>
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    //Function to add the new element:
    await dbContext.AddAsync(tarea);
    //Alternative way to add the element:
    //await dbContext.Tareas.AddAsync(tarea);

    //function to save the new element:
    await dbContext.SaveChangesAsync();
    return Results.Ok();
});


app.MapPut("/api/tareasupdate/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea,[FromRoute] Guid id ) =>
{
    //finding the Tarea 
    var currentTarea = dbContext.Tareas.Find(id);

    if(currentTarea!= null){
        currentTarea.CategoriaId = tarea.CategoriaId;
        currentTarea.Titulo = tarea.Titulo;
        currentTarea.PrioridadTarea = tarea.PrioridadTarea;
        currentTarea.Descripcion = tarea.Descripcion;

        //Confirmar los datos ingresados:
        await dbContext.SaveChangesAsync();
        return Results.Ok();
    }
   
    return Results.NotFound();
});

app.MapDelete("/api/tareasdelete/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id ) =>
{
    //Finding the tarea to delete
    var currentTarea = dbContext.Tareas.Find(id);
     //If it was found
     if(currentTarea!= null){
        dbContext.Remove(currentTarea);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
     }

     return Results.NotFound();
});

app.Run();
