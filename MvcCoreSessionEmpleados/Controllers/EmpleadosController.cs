using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MvcCoreSessionEmpleados.Extensions;
using MvcCoreSessionEmpleados.Models;
using MvcCoreSessionEmpleados.Repositories;

namespace MvcCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        private IMemoryCache cache;

        public EmpleadosController(RepositoryEmpleados repo, IMemoryCache cache)
        {
            this.repo = repo;
            this.cache = cache;
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if (salario != null)
            {
                //QUEREMOS ALMACENAR LA SUMA TOTAL DE SALARIOS
                //QUE TENGAMOS EN SESSION
                int sumaTotal = 0;
                if (HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    //SI YA TENEMOS DATOS ALMACENADOS, LOS RECUPERAMOS
                    sumaTotal =
                        HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                //SUMAMOS EL NUEVO SALARIO A LA SUMA TOTAL
                sumaTotal += salario.Value;
                //ALMACENAMOS EL VALOR DENTRO DE SESSION
                HttpContext.Session.SetObject("SUMASALARIAL", sumaTotal);
                ViewData["MENSAJE"] = "Salario almacenado: " + salario;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleados
            (int? idempleado)
        {
            if (idempleado != null)
            {
                Empleado empleado =
                    await this.repo.FindEmpleadoAsync(idempleado.Value);
                //EN SESSION TENDREMOS ALMACENADOS UN CONJUNTO DE EMPLEADOS
                List<Empleado> empleadosList;

                //DEBEMOS PREGUNTAR SI YA TENEMOS EMPLEADOS EN SESSION
                if (HttpContext.Session.GetObject<List<Empleado>>
                    ("EMPLEADOS") != null)
                {
                    //RECUPERAMOS LA LISTA DE SESSION
                    empleadosList =
                    HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    //CREAMOS UNA NUEVA LISTA PARA ALMACENAR LOS EMPLEADOS
                    empleadosList = new List<Empleado>();
                }
                //AGREGAMOS EL EMPLEADO AL LIST
                empleadosList.Add(empleado);
                //ALMACENAMOS LA LISTA EN SESSION
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["MENSAJE"] = "Empleado " + empleado.Apellido
                    + " almacenado correctamente";
            }
            List<Empleado> empleados =
                await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleadosOk
            (int? idempleado)
        {
            if (idempleado != null)
            {
                //ALMACENAMOS LO MINIMO...
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>
                    ("IDSEMPLEADOS") != null)
                {
                    //RECUPERAMOS LA COLECCION
                    idsEmpleados =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    //CREAMOS LA COLECCION
                    idsEmpleados = new List<int>();
                }
                //ALMACENAMOS EL ID DEL EMPLEADO
                idsEmpleados.Add(idempleado.Value);
                //ALMACENAMOS EN SESSION LOS DATOS
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + idsEmpleados.Count;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOk()
        {
            //RECUPERAMOS LA COLECCION DE SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en Session";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult> SessionEmpleadosV4
            (int? idempleado)
        {
            if (idempleado != null)
            {
                //ALMACENAMOS LO MINIMO...
                List<int> idsEmpleadosList;
                if (HttpContext.Session.GetObject<List<int>>
                    ("IDSEMPLEADOS") != null)
                {
                    //RECUPERAMOS LA COLECCION
                    idsEmpleadosList =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    //CREAMOS LA COLECCION
                    idsEmpleadosList = new List<int>();
                }
                //ALMACENAMOS EL ID DEL EMPLEADO
                idsEmpleadosList.Add(idempleado.Value);
                //ALMACENAMOS EN SESSION LOS DATOS
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleadosList);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + idsEmpleadosList.Count;
            }
            //PARA EL DIBUJO, DEBEMOS COMPROBAR SI EXISTE SESSION O NO
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
                return View(empleados);
            }
            else
            {
                List<Empleado> empleados = await
                    this.repo.GetEmpleadosNotSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            //RECUPERAMOS LA COLECCION DE SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen empleados en Session";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult> SessionEmpleadosV5
            (int? idempleado, int? idfavorito)
        {
            if (idfavorito != null)
            {
                //como estoy almacenando en cache, vamos a guardar los obj en lugar de los ids
                List<Empleado> favs;
                if (cache.Get("FAVORITOS") == null)
                {
                    //no existe nada en cache
                    favs = new List<Empleado>();
                }
                else
                {
                    //recuperamos el cache
                    favs = cache.Get<List<Empleado>>("FAVORITOS");
                }
                //buscamos al empleado para guardarlo
                Empleado fav = await this.repo.FindEmpleadoAsync(idfavorito.Value);
                favs.Add(fav);
                cache.Set("FAVORITOS", favs);
            }

            if (idempleado != null)
            {
                //ALMACENAMOS LO MINIMO...
                List<int> idsEmpleadosList;
                if (HttpContext.Session.GetObject<List<int>>
                    ("IDSEMPLEADOS") != null)
                {
                    //RECUPERAMOS LA COLECCION
                    idsEmpleadosList =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    //CREAMOS LA COLECCION
                    idsEmpleadosList = new List<int>();
                }
                //ALMACENAMOS EL ID DEL EMPLEADO
                idsEmpleadosList.Add(idempleado.Value);
                //ALMACENAMOS EN SESSION LOS DATOS
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleadosList);
                ViewData["MENSAJE"] = "Empleados almacenados: "
                    + idsEmpleadosList.Count;
            }

            List<Empleado> empleados = await repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV5(int? ideliminar)
        {
            //RECUPERAMOS LA COLECCION DE SESSION
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleados == null)
            {
                return View();
            }
            else
            {
                //preguntamos si hemos reibido algun dato para eliminar
                if (ideliminar != null)
                {
                    idsEmpleados.Remove(ideliminar.Value);
                    //si no tenemos expleados en session, nuestra coleccion existe y se queda a 0
                    //eliminamos session
                    if (idsEmpleados.Count == 0)
                    {
                        HttpContext.Session.Remove("IDSEMPLEADOS");
                    }
                    else
                    {
                        //actualizamos session
                        HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                    }
                }

                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public IActionResult Favoritos(int? ideliminar)
        {

            if (ideliminar != null)
            {
                List<Empleado> favs = cache.Get<List<Empleado>>("FAVORITOS");
                Empleado emp = favs.Find(z => z.IdEmpleado == ideliminar.Value);
                favs.Remove(emp);

                if (favs.Count == 0)
                {
                    cache.Remove("FAVORITOS");
                }
                else
                {
                    cache.Set("FAVORITOS", favs);
                }
            }

            return View();
        }
    }
}
