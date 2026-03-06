using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MvcCubosCarritoJuernes.Extensions;
using MvcCubosCarritoJuernes.Models;
using MvcCubosCarritoJuernes.Repositories;
using System.Threading.Tasks;

namespace MvcCubosCarritoJuernes.Controllers
{
    public class CubosController : Controller
    {
        private CubosRepository repo;
        private IMemoryCache memoryCache;

        public CubosController(CubosRepository repo, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.memoryCache = memoryCache;
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

        public async Task<IActionResult> Favoritos(int? idFavorito)
        {
            List<Cubo> cubosFavoritos = this.memoryCache.Get<List<Cubo>>("FAVORITOS") ?? new List<Cubo>();

            if (idFavorito != null)
            {
                Cubo cuboFavorito = await this.repo.GetCuboAsync(idFavorito.Value);
                if (cuboFavorito != null)
                {
                    cubosFavoritos.Add(cuboFavorito);
                    this.memoryCache.Set("FAVORITOS", cubosFavoritos);
                    Console.WriteLine("Estos son los cubos:" + cubosFavoritos);
                }
            }

            return View(cubosFavoritos);
        }
        public async Task<IActionResult> EliminarFavorito(int? idEliminar)
        {
            if(idEliminar != null)
            {
                List<Cubo> cubosFavoritos = this.memoryCache.Get<List<Cubo>>("FAVORITOS") ?? new List<Cubo>();
                Cubo cuboEliminado = cubosFavoritos.Find(c=> c.IdCubo == idEliminar.Value);
                cubosFavoritos.Remove(cuboEliminado);
                if (cubosFavoritos.Count == 0)
                {
                    this.memoryCache.Remove("FAVORITOS");    
                }
                else
                {
                    this.memoryCache.Set("FAVORITOS", cubosFavoritos);
                }
            }
            return RedirectToAction("Favoritos");
        }

    }
}
