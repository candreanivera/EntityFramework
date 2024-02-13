using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace primerproyecto.Models;

public class Tarea{
    [Key]
    public Guid TareaId {get;set;}
    [ForeignKey("CategoriaID")]
    public Guid CategoriaId {get;set;}
    [Required]
    [MaxLength(200)]
    public string Titulo {get; set;}
    public string descripcion {get; set;}
    public Prioridad PrioridadTarea {get; set;}
    public DateTime FechaCreacion {get; set;}

    //Propiedad virtual del tipo Categoria, llamada categoría
    public virtual Categoria Categoria {get; set;}

    [NotMapped]
    public string Resumen {get; set;}
}

public enum Prioridad {
    Baja,
    Media,
    Alta
}