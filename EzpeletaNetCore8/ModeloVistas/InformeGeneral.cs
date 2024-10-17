using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzpeletaNetCore8.Models;


public class ListadoEventosN1 
{
    public int EventoID { get; set; }
    public string? Descripcion {get; set; }
    public List<ListadoLugaresN2>? ListadoLugaresN2 { get; set; }
}

public class ListadoLugaresN2 
{
    public int LugarID { get; set; }
    public string? Descripcion {get; set; }
    public List<ListadoTipoEjerciciosN3>? ListadoTipoEjerciciosN3 { get; set; }
}

public class ListadoTipoEjerciciosN3 
{
    public int TipoEjercicioID { get; set; }
    public string? Descripcion {get; set; }
    public List<VistaEjercicioFisico>? VistaEjercicioFisico { get; set; }
}