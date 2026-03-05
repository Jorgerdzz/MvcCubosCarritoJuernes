using Microsoft.AspNetCore.Mvc;
using MvcCubosCarritoJuernes.Extensions;
using MvcCubosCarritoJuernes.Models;
using MvcCubosCarritoJuernes.Repositories;
using System.Threading.Tasks;

namespace MvcCubosCarritoJuernes.Controllers
{
    public class CubosController : Controller
    {
        private CubosRepository repo;

        public CubosController(CubosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos = await this.repo.GetCubosAsync();
            return View(cubos);
        }

        public async Task<IActionResult> Details(int idCubo)
        {
            Cubo cubo = await this.repo.GetCuboAsync(idCubo);
            return View(cubo);
        }

        public async Task<IActionResult> Update(int idCubo)
        {
            Cubo cubo = await this.repo.GetCuboAsync(idCubo);
            return View(cubo);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Cubo cubo)
        {
            await this.repo.UpdateCuboAsync(cubo);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cubo cubo)
        {
            await this.repo.InsertCuboAsync(cubo);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Guardar(int? idCubo)
        {
            if(idCubo != null)
            {
                List<int> idsCubos;
                if(HttpContext.Session.GetObject<List<int>>("IDSCUBOS") != null)
                {
                    idsCubos = HttpContext.Session.GetObject<List<int>>("IDSCUBOS");
                }
                else
                {
                    idsCubos = new List<int>();
                }
                idsCubos.Add(idCubo.Value);
                HttpContext.Session.SetObject("IDSCUBOS", idsCubos);
            }
            return RedirectToAction("Index", "Carrito");
        }
    }
}
