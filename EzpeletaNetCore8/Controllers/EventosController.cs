using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzpeletaNetCore8.Controllers;

[Authorize]
public class EventosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EventosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoEventos(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var eventos = _context.Eventos.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            eventos = eventos.Where(t => t.EventoID == id).ToList();
        }

        return Json(eventos);
    }

    public JsonResult GuardarEvento(int eventoID, string descripcion)
    {
        //1- VERIFICAMOS SI REALMENTE INGRESO ALGUN CARACTER Y LA VARIABLE NO SEA NULL
        // if (descripcion != null && descripcion != "")
        // {
        //     //INGRESA SI ESCRIBIO SI O SI
        // }

        // if (String.IsNullOrEmpty(descripcion) == false)
        // {
        //     //INGRESA SI ESCRIBIO SI O SI 
        // }

        string resultado = "";

        if (!String.IsNullOrEmpty(descripcion))
        {
            descripcion = descripcion.ToUpper();
            //INGRESA SI ESCRIBIO SI O SI 

            //2- VERIFICAR SI ESTA EDITANDO O CREANDO NUEVO REGISTRO
            if (eventoID == 0)
            {
                //3- VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //PARA REALIZAR ESA VERIFICACION BUSCAMOS EN EL CONTEXTO, ES DECIR EN BASE DE DATOS 
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION  
                var existeEvento = _context.Eventos.Where(t => t.Descripcion == descripcion).Count();
                if (existeEvento == 0)
                {
                    //4- GUARDAR EL TIPO DE EJERCICIO
                    var evento = new Evento
                    {
                        Descripcion = descripcion
                    };
                    _context.Add(evento);
                    _context.SaveChanges();
                }
                else
                {
                    resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                }
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var eventoEditar = _context.Eventos.Where(t => t.EventoID == eventoID).SingleOrDefault();
                if (eventoEditar != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO AL QUE ESTAMOS EDITANDO
                    var existeTipoEjercicio = _context.Eventos.Where(t => t.Descripcion == descripcion && t.EventoID != eventoID).Count();
                    if (existeTipoEjercicio == 0)
                    {
                        //QUIERE DECIR QUE EL ELEMENTO EXISTE Y ES CORRECTO ENTONCES CONTINUAMOS CON EL EDITAR
                        eventoEditar.Descripcion = descripcion;
                        _context.SaveChanges();
                    }
                    else
                    {
                        resultado = "YA EXISTE UN REGISTRO CON LA MISMA DESCRIPCIÓN";
                    }
                }
            }
        }
        else
        {
            resultado = "DEBE INGRESAR UNA DESCRIPCIÓN.";
        }

        return Json(resultado);
    }

    public JsonResult EliminarEvento(int eventoID)
    {
        bool eliminado = false;

        //BUSCAR SI EXISTEN EJERCICIOS CARGADOS
        var existe = _context.EjerciciosFisicos.Where(t => t.EventoID == eventoID).Count();
        if (existe == 0)
        {
            var evento = _context.Eventos.Find(eventoID);
            _context.Remove(evento);
            _context.SaveChanges();
            eliminado = true;
        }

        return Json(eliminado);
    }
}
