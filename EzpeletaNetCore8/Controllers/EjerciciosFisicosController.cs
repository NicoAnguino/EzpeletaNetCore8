using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EzpeletaNetCore8.Controllers;

[Authorize]
public class EjerciciosFisicosController : Controller
{
    private ApplicationDbContext _context;

    //CONSTRUCTOR
    public EjerciciosFisicosController(ApplicationDbContext context)
    {
        _context = context;
    }


    public IActionResult EjerciciosPorTipo()
    {
          // Crear una lista de SelectListItem que incluya el elemento adicional
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]" }
        };

        // Obtener todas las opciones del enum
        var enumValues = Enum.GetValues(typeof(EstadoEmocional)).Cast<EstadoEmocional>();

        // Convertir las opciones del enum en SelectListItem
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        // Pasar la lista de opciones al modelo de la vista
        ViewBag.EstadoEmocionalInicio = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFin = selectListItems.OrderBy(t => t.Text).ToList();

        var tipoEjercicios = _context.TipoEjercicios.ToList();
        var tiposEjerciciosBuscar = tipoEjercicios.ToList();

        tipoEjercicios.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        tiposEjerciciosBuscar.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[TODOS]" });
        ViewBag.TipoEjercicioBuscarID = new SelectList(tiposEjerciciosBuscar.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }


      public JsonResult MostrarEjerciciosPorTipo(DateTime? FechaDesdeBuscar, DateTime? FechaHastaBuscar)
    {
        //INICIALIZAMOS EL LISTADO DE ELEMENTOS VACIOS
        List<VistaTipoEjercicio> tiposEjerciosMostrar = new List<VistaTipoEjercicio>();

        //BUSCAMOS EL LISTADO COMPLETO DE EJERCICIOS FISICOS
        var ejerciciosFisicos = _context.EjerciciosFisicos.Include(t => t.TipoEjercicio).ToList();

        //FILTRAMOS POR FECHA EN EL CASO DE QUE SEAN DISTINTOS DE NULO
        if(FechaDesdeBuscar != null && FechaHastaBuscar != null)
        {
             ejerciciosFisicos = ejerciciosFisicos.Where(t => t.Inicio >= FechaDesdeBuscar && t.Inicio <= FechaHastaBuscar).ToList();
        }

        //RECORREMOS LOS EJERCICIOS ORDENADOS POR DESCRIPCION DE TIPO DE EJERCICIO Y LUEGO POR FECHA DE INICIO
        foreach (var e in ejerciciosFisicos.OrderBy(t => t.Inicio).OrderBy(t => t.TipoEjercicio.Descripcion))
        {
           
            //POR CADA EJERCICIO BUSCAR SI EXISTE EN EL LISTADO EL TIPO DE EJERCICIO 
            var tipoEjercicioMostrar = tiposEjerciosMostrar.Where(t => t.TipoEjercicioID == e.TipoEjercicioID).SingleOrDefault();
            if(tipoEjercicioMostrar == null){
                //SI NO EXISTE, AGREGARLO AL LISTADO 
                tipoEjercicioMostrar = new VistaTipoEjercicio
                {
                    TipoEjercicioID = e.TipoEjercicioID,
                    Descripcion = e.TipoEjercicio.Descripcion,
                    ListadoEjercicios = new List<VistaEjercicioFisico>()
                };
                tiposEjerciosMostrar.Add(tipoEjercicioMostrar);
            }

            
            //LUEGO ARMAMOS EL OBJETO DE SEGUNDO NIVEL CON LOS DATOS DEL EJERCICIO
            var vistaEjercicio = new VistaEjercicioFisico
            {
                 EjercicioFisicoID = e.EjercicioFisicoID,
            TipoEjercicioID = e.TipoEjercicioID,
            TipoEjercicioNombre = e.TipoEjercicio.Descripcion,
            Inicio = e.Inicio,
            InicioString = e.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Fin = e.Fin,
            FinString = e.Fin.ToString("dd/MM/yyyy HH:mm"),
            IntervaloEjercicio = e.IntervaloEjercicio,
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinString = e.EstadoEmocionalFin.ToString().ToUpper(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioString = e.EstadoEmocionalInicio.ToString().ToUpper(),

            //ATENCIÓN A LA CONDICION PARA MOSTRAR O NO LA OBSERVACIÓN
            Observaciones = String.IsNullOrEmpty(e.Observaciones) ? "" : e.Observaciones
            };

            //LUEGO AGREGAMOS ESE OBJETO DE EJERCICIO AL LISTADO DE EJERCICIOS DE ESE TIPO DE EJERCICIO CORRESPONDIENTE
            tipoEjercicioMostrar.ListadoEjercicios.Add(vistaEjercicio);          
        }

       return Json(tiposEjerciosMostrar);
    }


    public IActionResult Index()
    {   
        // Crear una lista de SelectListItem que incluya el elemento adicional
        var selectListItems = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "[SELECCIONE...]" }
        };

        // Obtener todas las opciones del enum
        var enumValues = Enum.GetValues(typeof(EstadoEmocional)).Cast<EstadoEmocional>();

        // Convertir las opciones del enum en SelectListItem
        selectListItems.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        // Pasar la lista de opciones al modelo de la vista
        ViewBag.EstadoEmocionalInicio = selectListItems.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFin = selectListItems.OrderBy(t => t.Text).ToList();

        var tipoEjercicios = _context.TipoEjercicios.ToList();
        var tiposEjerciciosBuscar = tipoEjercicios.ToList();

        tipoEjercicios.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[SELECCIONE...]" });
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        tiposEjerciciosBuscar.Add(new TipoEjercicio { TipoEjercicioID = 0, Descripcion = "[TODOS]" });
        ViewBag.TipoEjercicioBuscarID = new SelectList(tiposEjerciciosBuscar.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }


  

    public JsonResult GetEjerciciosFisicos(int? id, DateTime? FechaDesdeBuscar, DateTime? FechaHastaBuscar, int? TipoEjercicioBuscarID)
    {
        var ejerciciosFisicos = _context.EjerciciosFisicos.Include(t => t.TipoEjercicio).ToList();
        if (id != null)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.EjercicioFisicoID == id).ToList();
        }

        if(FechaDesdeBuscar != null && FechaHastaBuscar != null){
             ejerciciosFisicos = ejerciciosFisicos.Where(t => t.Inicio >= FechaDesdeBuscar && t.Inicio <= FechaHastaBuscar).ToList();
        }

        if (TipoEjercicioBuscarID != null && TipoEjercicioBuscarID != 0)
        {
            ejerciciosFisicos = ejerciciosFisicos.Where(t => t.TipoEjercicioID == TipoEjercicioBuscarID).ToList();
        }

        ejerciciosFisicos = ejerciciosFisicos.OrderByDescending(t => t.Inicio).ToList();

        var ejercicioFisicosMostrar = ejerciciosFisicos
        .Select(e => new VistaEjercicioFisico
        {
            EjercicioFisicoID = e.EjercicioFisicoID,
            TipoEjercicioID = e.TipoEjercicioID,
            TipoEjercicioNombre = e.TipoEjercicio.Descripcion,
            Inicio = e.Inicio,
            InicioString = e.Inicio.ToString("dd/MM/yyyy HH:mm"),
            Fin = e.Fin,
            FinString = e.Fin.ToString("dd/MM/yyyy HH:mm"),
            EstadoEmocionalFin = e.EstadoEmocionalFin,
            EstadoEmocionalFinString = e.EstadoEmocionalFin.ToString().ToUpper(),
            EstadoEmocionalInicio = e.EstadoEmocionalInicio,
            EstadoEmocionalInicioString = e.EstadoEmocionalInicio.ToString().ToUpper(),
            Observaciones = e.Observaciones
        })
        .ToList();


        return Json(ejercicioFisicosMostrar);
    }

    public JsonResult GuardarEjercicio(int ejercicioFisicoID, int tipoEjercicioID, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, DateTime fechaInicio, DateTime fechaFin, string? observaciones)
    {
        int error = 0;

        //VALIDAMOS QUE SELECCIONE TIPO DE EJERCICIO
        if (tipoEjercicioID == 0)
        {
            error = 1;
        }

        //VALIDAMOS QUE LA FECHA DE INICIO NO SEA MAYOR A LA DE FIN
        if(error == 0)
        {        
            if (ejercicioFisicoID == 0)
            {
                //4- GUARDAR EL EJERCICIO
                var ejercicio = new EjercicioFisico
                {
                    TipoEjercicioID = tipoEjercicioID,
                    EstadoEmocionalInicio = estadoEmocionalInicio,
                    EstadoEmocionalFin = estadoEmocionalFin,
                    Inicio = fechaInicio,
                    Fin = fechaFin,
                    Observaciones = observaciones
                };
                _context.Add(ejercicio);
                _context.SaveChanges();
            }
            else
            {
                //QUIERE DECIR QUE VAMOS A EDITAR EL REGISTRO
                var ejercicioEditar = _context.EjerciciosFisicos.Where(t => t.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();
                if (ejercicioEditar != null)
                {
                    ejercicioEditar.TipoEjercicioID = tipoEjercicioID;
                    ejercicioEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                    ejercicioEditar.EstadoEmocionalFin = estadoEmocionalFin;
                    ejercicioEditar.Inicio = fechaInicio;
                    ejercicioEditar.Fin = fechaFin;
                    ejercicioEditar.Observaciones = observaciones;
                    _context.SaveChanges();
                }
            }
        }

        return Json(error);
    }

    public JsonResult EliminarEjercicio(int ejercicioFisicoID)
    {
        var ejercicio = _context.EjerciciosFisicos.Find(ejercicioFisicoID);
        _context.Remove(ejercicio);
        _context.SaveChanges();

        return Json(true);
    }
}