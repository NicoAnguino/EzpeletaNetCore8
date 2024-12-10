using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzpeletaNetCore8.Models;

public class Deportista
{
    [Key]
    public int DeportistaID { get; set; }
    public string? NombreCompleto { get; set; } 
    public DateTime FechaNacimiento { get; set; }
    public Genero Genero {get; set; }
    public decimal Peso {get; set; }
    public decimal? Altura {get; set; }
    public string? UsuarioID { get; set; }   
}

public enum Genero{
    Masculino = 1,
    Femenino
}