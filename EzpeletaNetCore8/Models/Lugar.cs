using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzpeletaNetCore8.Models;

public class Lugar
{
    [Key]
    public int LugarID { get; set; }
    public string? Descripcion { get; set; } 
    public bool Eliminado { get; set; }
    public string? UsuarioID { get; set; }  
    public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; } 
}

//TIPO PROFESION
// public class VistaTipoEjercicio
// {   
//      public int TipoEjercicioID { get; set; }
//      public string? Descripcion { get; set; }
//      public List<VistaEjercicioFisico>? ListadoEjercicios { get; set; }
// }

