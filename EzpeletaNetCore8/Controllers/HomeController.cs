using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using Microsoft.AspNetCore.Authorization;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EzpeletaNetCore8.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var tipoEjercicios = _context.TipoEjercicios.ToList();
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");

        return View();
    }

    public JsonResult GraficoTipoEjercicioMes(int TipoEjercicioID, int Mes, int Anio)
    {
        List<EjerciciosPorDia> ejerciciosPorDias = new List<EjerciciosPorDia>();

        //POR DEFECTO EN EL LISTADO AGREGAR TODOS LOS DIAS DEL MES PARA LUEGO RECORRER Y COMPLETAR EN BASE A LOS DIAS CON EJERCICIOS
        var diasDelMes = DateTime.DaysInMonth(Anio, Mes);

        //INICIALIZO UNA VARIABLE DE TIPO FECHA
        DateTime fechaMes = new DateTime();
        //RESTAMOS UN MES SOBRE ESA FECHA
        fechaMes = fechaMes.AddMonths(Mes - 1);

        for (int i = 1; i <= diasDelMes; i++)
        {
            var diaMesMostrar = new EjerciciosPorDia
            {
                Dia = i,
                Mes = fechaMes.ToString("MMMM").ToUpper(),
                CantidadMinutos = 0
            };
            ejerciciosPorDias.Add(diaMesMostrar);
        }

        //DEBEMOS BUSCAR EN BASE DE DATOS EN LA TABLA DE EJERCICIOS LOS EJERCICIOS QUE COINCIDAN CON LOS PARAMETROS INGRESADOS
        var ejercicios = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == TipoEjercicioID
          && e.Inicio.Month == Mes && e.Inicio.Year == Anio).ToList();

        foreach (var ejercicio in ejercicios.OrderBy(e => e.Inicio))
        {
            //POR CADA EJERCICIO DEBEMOS AGREGAR EN EL LISTADO SI EL DIA DE ESE EJERCICIO NO EXISTE, SINO SUMARLO
            var ejercicioDiaMostrar = ejerciciosPorDias.Where(e => e.Dia == ejercicio.Inicio.Day).SingleOrDefault();
            if (ejercicioDiaMostrar != null)
            {
                ejercicioDiaMostrar.CantidadMinutos += ejercicio.Fin.Subtract(ejercicio.Inicio).Minutes;
            }
        }

        return Json(ejerciciosPorDias);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
