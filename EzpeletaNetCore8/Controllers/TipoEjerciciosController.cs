using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzpeletaNetCore8.Controllers;

//[Authorize]
public class TipoEjerciciosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public TipoEjerciciosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoTipoEjercicios(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var tipoDeEjercicios = _context.TipoEjercicios.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            tipoDeEjercicios = tipoDeEjercicios.Where(t => t.TipoEjercicioID == id).ToList();
        }

        return Json(tipoDeEjercicios);
    }

    public JsonResult GuardarTipoEjercicio(int tipoEjercicioID, string descripcion)
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

        if (!String.IsNullOrEmpty(descripcion))
        {
            //INGRESA SI ESCRIBIO SI O SI 

            //2- VERIFICAR SI ESTA EDITANDO O CREANDO NUEVO REGISTRO
            if (tipoEjercicioID == 0)
            {
                //3- VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //PARA REALIZAR ESA VERIFICACION BUSCAMOS EN EL CONTEXTO, ES DECIR EN BASE DE DATOS 
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION  
                var existeTipoEjercicio = _context.TipoEjercicios.Where(t => t.Descripcion == descripcion).Count();
                if (existeTipoEjercicio == 0)
                {
                    //4- GUARDAR EL TIPO DE EJERCICIO
                    var tipoEjercicio = new TipoEjercicio
                    {
                        Descripcion = descripcion.ToUpper()
                    };
                    _context.Add(tipoEjercicio);
                    _context.SaveChanges();
                }
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var tipoEjercicioEditar = _context.TipoEjercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).SingleOrDefault();
                if(tipoEjercicioEditar != null){
                    //QUIERE DECIR QUE EL ELEMENTO EXISTE Y ES CORRECTO ENTONCES CONTINUAMOS CON EL EDITAR
                    tipoEjercicioEditar.Descripcion = descripcion.ToUpper();
                    _context.SaveChanges();
                }
            }

        }

        return Json(true);
    }

    public JsonResult EliminarTipoEjercicio(int tipoEjercicioID)
    {
        var tipoEjercicio = _context.TipoEjercicios.Find(tipoEjercicioID);
        _context.Remove(tipoEjercicio);
        _context.SaveChanges();

        return Json(true);
    }
}
