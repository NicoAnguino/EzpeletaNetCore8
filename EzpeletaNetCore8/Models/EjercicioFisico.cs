using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EzpeletaNetCore8.Models;

namespace EzpeletaNetCore8.Models
{
    public class EjercicioFisico
    {
        [Key]
        public int EjercicioFisicoID { get; set; }
        public int TipoEjercicioID { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }

        [NotMapped]
        public TimeSpan IntervaloEjercicio { get {return Fin - Inicio;} }//UN CAMPO EN VISTA SOLAMENTE NO EN LA TABLA DE SQL

        public EstadoEmocional EstadoEmocionalInicio {get; set; } 
        public EstadoEmocional EstadoEmocionalFin {get; set; } 
        public string? Observaciones {get; set; }

        public int LugarID { get; set; }

        public int EventoID { get; set; }

        public virtual TipoEjercicio TipoEjercicio { get; set; }
        public virtual Lugar Lugar { get; set; }
        //public virtual Evento Evento { get; set; }
    }

    public enum EstadoEmocional{
        Feliz = 1,
        Triste,
        Enojado,
        Ansioso,
        Estresado,
        Relajado,
        Aburrido,
        Emocionado,
        Agobiado,
        Confundido,
        Optimista,
        Pesimista,
        Motivado,
        Cansado,
        Eufórico,
        Agitado,
        Satisfecho,
        Desanimado
    }

    public class VistaEjercicioFisico
    {
        public int EjercicioFisicoID { get; set; }
        public int TipoEjercicioID { get; set; }
        public string? TipoEjercicioNombre {get;set;}
        public int LugarID { get; set; }
        public string? LugarNombre { get; set; }
        public DateTime Inicio { get; set; }
        public string? InicioString { get; set; }

        public TimeSpan IntervaloEjercicio { get; set; }
         public DateTime Fin { get; set; }
        public string? FinString { get; set; }
        public EstadoEmocional EstadoEmocionalInicio {get; set; } 
        public EstadoEmocional EstadoEmocionalFin {get; set; } 
        public string? EstadoEmocionalInicioString {get; set; } 
        public string? EstadoEmocionalFinString {get; set; } 
        public string? Observaciones {get; set; }
    }
    public class VistaSumaEjercicioFisico
    {
        public string? TipoEjercicioNombre {get; set;}
        public int TotalidadMinutos {get; set; }
        public int TotalidadDiasConEjercicio {get;set;}
        public int TotalidadDiasSinEjercicio {get;set;}

        public List<VistaEjercicioFisico>? DiasEjercicios {get;set;}
    }

    public class VistaPorDiaEjercicioFisico
    {   
        public int Anio {get; set; }  
        public string? Mes { get; set; }
        public int? Dia { get; set; }
        public int CantidadMinutos { get; set; }
    }
}