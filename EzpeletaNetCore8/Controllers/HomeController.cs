using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EzpeletaNetCore8.Models;
using Microsoft.AspNetCore.Authorization;
using EzpeletaNetCore8.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;

namespace EzpeletaNetCore8.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _rolManager = rolManager;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
      
        var tipoEjercicios = _context.TipoEjercicios.ToList();
        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Descripcion), "TipoEjercicioID", "Descripcion");
        
        await InicializarPermisosUsuario();
        
        return View();
    }

    public JsonResult PanelGraficos(int TipoEjercicioID, int Mes, int Anio)
    {
        PanelEjercicios panelEjercicios = new PanelEjercicios();
        panelEjercicios.EjerciciosPorDias = new List<EjerciciosPorDia>();
        panelEjercicios.VistaTipoEjercicioFisico = new List<VistaTipoEjercicioFisico>();

        //INICIO GRAFICO DIAS DEL MES
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
            panelEjercicios.EjerciciosPorDias.Add(diaMesMostrar);
        }

        //DEBEMOS BUSCAR EN BASE DE DATOS EN LA TABLA DE EJERCICIOS LOS EJERCICIOS QUE COINCIDAN CON EL MES Y AÑO INGRESADO
        var ejerciciosMes = _context.EjerciciosFisicos.Where(e => e.Inicio.Month == Mes && e.Inicio.Year == Anio).ToList();

        var ejerciciosTipoEjercicio = ejerciciosMes.Where(e=>e.TipoEjercicioID == TipoEjercicioID).ToList();

        foreach (var ejercicio in ejerciciosTipoEjercicio.OrderBy(e => e.Inicio))
        {
            //POR CADA EJERCICIO DEBEMOS AGREGAR EN EL LISTADO SI EL DIA DE ESE EJERCICIO NO EXISTE, SINO SUMARLO
            var ejercicioDiaMostrar = panelEjercicios.EjerciciosPorDias.Where(e => e.Dia == ejercicio.Inicio.Day).SingleOrDefault();
            if (ejercicioDiaMostrar != null)
            {
                ejercicioDiaMostrar.CantidadMinutos += Convert.ToInt32(ejercicio.IntervaloEjercicio.TotalMinutes);
            }
        }
        //FIN GRAFICO DIAS DEL MES

        //INICIO GRAFICO TOTALIZADOR POR TIPO DE EJERCICIO

        //BUSCAMOS LOS TIPOS DE EJERCICIOS QUE EXISTEN ACTIVOS
        var tiposEjerciciosFisicos = _context.TipoEjercicios.Where(s => s.Eliminado == false).ToList();

        //LUEGO LOS RECORREMOS
        foreach (var tipoEjercicioFisico in tiposEjerciciosFisicos)
        {
            //POR CADA TIPO DE EJERCICIO BUSQUEMOS EN LA TABLA DE EJERCICIOS FISICOS POR ESE TIPO, EN EL MES Y AÑO SOLICITADO
            var ejercicios = _context.EjerciciosFisicos
                                .Where(s => s.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID
                                && s.Inicio.Month == Mes && s.Inicio.Year == Anio).ToList();

            foreach (var ejercicio in ejercicios)
            {
                var tipoEjercicioFisicoMostrar = panelEjercicios.VistaTipoEjercicioFisico.Where(e => e.TipoEjercicioID == tipoEjercicioFisico.TipoEjercicioID).SingleOrDefault();
                if (tipoEjercicioFisicoMostrar == null)
                {
                    tipoEjercicioFisicoMostrar = new VistaTipoEjercicioFisico
                    {
                        TipoEjercicioID = tipoEjercicioFisico.TipoEjercicioID,
                        Descripcion = tipoEjercicioFisico.Descripcion,
                        CantidadMinutos = Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes)
                    };
                    panelEjercicios.VistaTipoEjercicioFisico.Add(tipoEjercicioFisicoMostrar);
                }
                else{
                    tipoEjercicioFisicoMostrar.CantidadMinutos += Convert.ToDecimal(ejercicio.IntervaloEjercicio.TotalMinutes);
                }
            }
        }

        //FIN GRAFICO TOTALIZADOR POR TIPO DE EJERCICIO

        return Json(panelEjercicios);
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
                //TimeSpan diferencia = ejercicio.Fin - ejercicio.Inicio;
                //ejercicioDiaMostrar.CantidadMinutos += Convert.ToInt32(diferencia.TotalMinutes);
                ejercicioDiaMostrar.CantidadMinutos += Convert.ToInt32(ejercicio.IntervaloEjercicio.TotalMinutes);
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


     public async Task<JsonResult> InicializarPermisosUsuario()
    {
            //CREAR ROLES SI NO EXISTEN
            var nombreRolCrearExiste = _context.Roles.Where(r => r.Name == "ADMINISTRADOR").SingleOrDefault();
            if (nombreRolCrearExiste == null)
            {
                var roleResult = await _rolManager.CreateAsync(new IdentityRole("ADMINISTRADOR"));
            }

            //CREAR USUARIO PRINCIPAL
            bool creado = false;
            //BUSCAR POR MEDIO DE CORREO ELECTRONICO SI EXISTE EL USUARIO
            var usuario = _context.Users.Where(u => u.Email == "admin@sistema.com").SingleOrDefault();
            if (usuario == null)
            {
                var user = new IdentityUser { UserName = "admin@sistema.com", Email = "admin@sistema.com" };
                var result = await _userManager.CreateAsync(user, "password");

                await _userManager.AddToRoleAsync(user, "ADMINISTRADOR");
                creado = result.Succeeded;
            }

            //CODIGO PARA BUSCAR EL USUARIO EN CASO DE NECESITARLO
            var superusuario = _context.Users.Where(r => r.Email == "admin@sistema.com").SingleOrDefault();
            if (superusuario != null)
            {

                //var personaSuperusuario = _contexto.Personas.Where(r => r.UsuarioID == superusuario.Id).Count();

                 var usuarioID = superusuario.Id;
            
            }

            return Json(creado);
        }

    [Authorize(Roles = "Administrator")]
    public IActionResult PrivadaAdmin()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
