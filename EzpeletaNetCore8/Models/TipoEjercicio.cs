using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzpeletaNetCore8.Models;

public class TipoEjercicio
{
    [Key]
    public int TipoEjercicioID { get; set; }
    public string? Descripcion { get; set; } 
    public bool Eliminado { get; set; }
    public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; } 
}

//TIPO PROFESION
public class VistaTipoEjercicio
{   
     public int TipoEjercicioID { get; set; }
     public string? Descripcion { get; set; }
     public List<VistaEjercicios> ListadoPersonas { get; set; }
}

//PERSONAS QUE ESTAN REGISTRADAS EN SERVICIOS
public class VistaEjercicios{
    public string? NombrePersona { get; set; }
    public string? Telefono { get; set; }
}

