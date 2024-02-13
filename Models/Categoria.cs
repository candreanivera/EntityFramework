using System.ComponentModel.DataAnnotations;
namespace primerproyecto.Models;

public class Categoria {
    [Key]
    public Guid CategoriaId {get; set;}
    [Required]
    [MaxLength(150)]
    public string Nombre {get; set;}
    public string descripcion {get;set;}

    public virtual ICollection<Tarea> Tareas {get; set;}

}