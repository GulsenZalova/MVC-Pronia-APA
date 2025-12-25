using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;
using ProniaApp.ViewModels;

namespace ProniaApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;  

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Slides = await _context.Sliders.OrderBy(s=>s.Order).Take(2).ToListAsync(),
                Products=await _context.Products.Include(p=>p.ProductImages).ToListAsync()

            };
            return View(homeVM);
        }


        public IActionResult Details()
        {
            return View();
        }

    }
}



// Adonet dezavantaj
//1. cirkli kod sayilir
//2. sql daha yuksek biliglere teleb olunur
//3. Sql injection


// cox suretli
// cox murekkeb sorgular yazmaq mumkundur.



