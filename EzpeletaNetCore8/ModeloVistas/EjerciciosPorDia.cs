using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzpeletaNetCore8.Models;


public class PanelEjercicios 
{
    public List<EjerciciosPorDia>? EjerciciosPorDias { get; set; }
    public List<VistaTipoEjercicioFisico>? VistaTipoEjercicioFisico { get; set; }
}

public class EjerciciosPorDia
{   
    public int Dia { get; set; }
    public string? Mes { get; set; }
    public int CantidadMinutos { get; set; }    
}

public class VistaTipoEjercicioFisico
{
     public int TipoEjercicioID { get; set; }
     public string? Descripcion { get; set; } 

     public decimal CantidadMinutos { get; set; }

}

