using Microsoft.AspNetCore.Mvc;
using MvcCubosCarritoJuernes.Extensions;
using MvcCubosCarritoJuernes.Models;
using MvcCubosCarritoJuernes.Repositories;
using System.Threading.Tasks;

namespace MvcCubosCarritoJuernes.Controllers
{
    public class CarritoController : Controller
    {
        private CubosRepository repo;

        public CarritoController(CubosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index(int? idEliminar)
        {
            List<int> idsCubos = HttpContext.Session.GetObject<List<int>>("IDSCUBOS");
            if(idsCubos == null)
            {
                ViewData["MENSAJE"] = "No existen cubos en el carrito";
                return View();
            }
            else
            {
                if (idEliminar != null)
                {
                    idsCubos.Remove(idEliminar.Value);
                    if (idsCubos.Count == 0)
                    {
                        HttpContext.Session.Remove("IDSCUBOS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSCUBOS", idsCubos);
                    }
                }
                List<Cubo> cubos = await this.repo.GetCubosSessionAsync(idsCubos);
                int suma = cubos.Sum(x => x.Precio);
                ViewData["SUMA"] = suma;
                return View(cubos);
            }
        }

    }
}
