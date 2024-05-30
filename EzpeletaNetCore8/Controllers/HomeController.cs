using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using Microsoft.AspNetCore.Authorization;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLitePCL;

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
                Mes = fechaMes.ToString("MMM").ToUpper(),
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
                //CON LA CLASE TIMESPAN PODEMOS GUARDAR EL INTERVALO DE TIEMPO ENTRE DOS FECHAS PARA LUEGO USAR EL RESULTADO EN MINUTOS TOTALES
                TimeSpan diferencia = ejercicio.Fin - ejercicio.Inicio;
                ejercicioDiaMostrar.CantidadMinutos += Convert.ToInt32(diferencia.TotalMinutes);
            }
        }

        return Json(ejerciciosPorDias);
    }

    public JsonResult GraficoTortaTipoActividades(int Mes, int Anio)
    {
        //INICIALIZAMOS UN LISTADO DE TIPO DE EJERCICIOS
        var vistaTipoEjercicioFisico = new List<VistaTipoEjercicioFisico>();

        //BUSCAMOS LOS TIPOS DE EJERCICIOS QUE EXISTEN ACTIVOS
        var tiposEjerciciosFisicos = _context.TipoEjercicios.Where(s => s.Eliminado == false).ToList();

        //LUEGO LOS RECORREMOS
        foreach (var tipoEjercicioFisico in tiposEjerciciosFisicos)
        {
            //POR CADA TIPO DE EJERCICIO BUSQUEMOS EN LA TABLA DE EJERCICIOS FISICOS POR ESE TIPO, EN EL MES Y AÑO SOLICITADO
            var ejercicios = _context.EjerciciosFisicos
                                .Where(s => s.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID
                                && s.Inicio.Month == Mes && s.Inicio.Year == Anio).ToList();

            // .Select(e => e.IntervaloEjercicio.TotalMinutes)
            // .Sum();

            foreach (var ejercicio in ejercicios)
            {
                var tipoEjercicioFisicoMostrar = vistaTipoEjercicioFisico.Where(e => e.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID).SingleOrDefault();
                if (tipoEjercicioFisicoMostrar == null)
                {
                    tipoEjercicioFisicoMostrar = new VistaTipoEjercicioFisico
                    {
                        TipoEjercicioID = tipoEjercicioFisico.TipoEjercicioID,
                        Descripcion = tipoEjercicioFisico.Descripcion,
                        CantidadMinutos = Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes)
                    };
                    vistaTipoEjercicioFisico.Add(tipoEjercicioFisicoMostrar);
                }
                else{
                    tipoEjercicioFisicoMostrar.CantidadMinutos += Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes);
                }
            }


        }

        return Json(vistaTipoEjercicioFisico);
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
