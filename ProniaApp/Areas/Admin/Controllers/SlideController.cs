using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;

namespace ProniaApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SlideController : Controller
        
    {
         public readonly AppDbContext _context;
        public SlideController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            List<Slide> slides= await _context.Sliders.ToListAsync();
            return View(slides);
        }
        public ActionResult Test()
        {
            string result=Guid.NewGuid().ToString();
            return Content(result);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public  async Task<ActionResult> Create(Slide slide)
        {

            
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!slide.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Siz uygun formatta file elave etmirsiz");
                return View();
            }
            if (slide.Photo.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Photo","Siz duzgun hecmde file elave etmirsiz");
            }
            
            // f413bda4-54b6-4a7a-abbb-7422cbfd6e7fmy-flower.jpg
            string fileName=String.Concat(Guid.NewGuid().ToString(),slide.Photo.FileName);
            string path = "/Users/gulshanzalova/Desktop/MVCProjects-APA/ProniaApp/wwwroot/assets/images/website-images/" + fileName;
            FileStream fileStream = new(path,FileMode.Create);
            await slide.Photo.CopyToAsync(fileStream);
            fileStream.Close();
            slide.Image=fileName;

            // return Content(slide.Photo.FileName + " " + slide.Photo.ContentType + " " + slide.Photo.Length);
            await _context.Sliders.AddAsync(slide);
            await  _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
