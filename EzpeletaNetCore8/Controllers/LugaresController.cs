using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Authorization;

namespace EzpeletaNetCore8.Controllers;

[Authorize]
public class LugaresController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public LugaresController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public JsonResult ListadoLugares(int? id)
    {
        //DEFINIMOS UNA VARIABLE EN DONDE GUARDAMOS EL LISTADO COMPLETO DE TIPOS DE EJERCICIOS
        var lugares = _context.Lugares.ToList();

        //LUEGO PREGUNTAMOS SI EL USUARIO INGRESO UN ID
        //QUIERE DECIR QUE QUIERE UN EJERCICIO EN PARTICULAR
        if (id != null)
        {
            //FILTRAMOS EL LISTADO COMPLETO DE EJERCICIOS POR EL EJERCICIO QUE COINCIDA CON ESE ID
            lugares = lugares.Where(t => t.LugarID == id).ToList();
        }

        return Json(lugares);
    }

    public JsonResult GuardarLugar(int lugarID, string descripcion)
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
            if (lugarID == 0)
            {
                //3- VERIFICAMOS SI EXISTE EN BASE DE DATOS UN REGISTRO CON LA MISMA DESCRIPCION
                //PARA REALIZAR ESA VERIFICACION BUSCAMOS EN EL CONTEXTO, ES DECIR EN BASE DE DATOS 
                //SI EXISTE UN REGISTRO CON ESA DESCRIPCION  
                var existeLugar = _context.Lugares.Where(t => t.Descripcion == descripcion).Count();
                if (existeLugar == 0)
                {
                    //4- GUARDAR EL TIPO DE EJERCICIO
                    var lugar = new Lugar
                    {
                        Descripcion = descripcion
                    };
                    _context.Add(lugar);
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
                var lugarEditar = _context.Lugares.Where(t => t.LugarID == lugarID).SingleOrDefault();
                if (lugarEditar != null)
                {
                    //BUSCAMOS EN LA TABLA SI EXISTE UN REGISTRO CON EL MISMO NOMBRE PERO QUE EL ID SEA DISTINTO AL QUE ESTAMOS EDITANDO
                    var existeTipoEjercicio = _context.Lugares.Where(t => t.Descripcion == descripcion && t.LugarID != lugarID).Count();
                    if (existeTipoEjercicio == 0)
                    {
                        //QUIERE DECIR QUE EL ELEMENTO EXISTE Y ES CORRECTO ENTONCES CONTINUAMOS CON EL EDITAR
                        lugarEditar.Descripcion = descripcion;
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

    public JsonResult EliminarLugar(int lugarID)
    {
        bool eliminado = false;

        //BUSCAR SI EXISTEN EJERCICIOS CARGADOS
        var existeLugar = _context.EjerciciosFisicos.Where(t => t.LugarID == lugarID).Count();
        if (existeLugar == 0)
        {
            var lugar = _context.Lugares.Find(lugarID);
            _context.Remove(lugar);
            _context.SaveChanges();
            eliminado = true;
        }

        return Json(eliminado);
    }
}
