using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.DAL;
using ProniaApp.Models;

namespace ProniaApp.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller

    {

        public readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()

        {

            List<Category> categories =await  _context.Categories.Include(c=>c.Products).ToListAsync();

            return View(categories);
        }



        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
          public async Task<ActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result= await _context.Categories.AnyAsync(c=>c.Name == category.Name);
            if (result)
            {
                ModelState.AddModelError("Name","Bu category name sistemde var artiq");
                return View();
            }
           await  _context.AddAsync(category);
           await _context.SaveChangesAsync();
           return RedirectToAction("Index");
        }


        public async Task<ActionResult> Update(int? id)
        {
            //bize gonderilen id ile databazadan categoryniu tapib adini update formda inpute yazmaq
            // riskler
            // id null gele biler
            // id menfi gele biler
            // id tam qaydasinda olar amma bele id-li mehsul databazada olmaz

            if(id==null || id < 1)
            {
                return BadRequest();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
     
            if (category is null)
            {
                return NotFound();
            }


            return View(category);
        }


       [HttpPost]
        public async Task<ActionResult> Update(int? id, Category category)
        {
              if(id==null || id < 1)
            {
                return BadRequest();
            }

            Category exists = await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
     
            if (exists is null)
            {
                return NotFound();
            }

            //riskler
            // bos gondere biler
            // eyni categorileri gondere biler
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result= await _context.Categories.AnyAsync(c=>c.Name==category.Name);
            if (result)
            {
                ModelState.AddModelError("Name","bu category sistemde var artiq");
                return View();
            }
            // exists databazadan getirdiyimiz deyismek istediyimiz category-dir
            // Category tipinden category ise update formdan bize gonderilen update olunmus categorydir.
            exists.Name=category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        
         public async Task<ActionResult> Delete(int? id)
        {
               if(id==null || id < 1)
            {
                return BadRequest();
            }

            Category category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
     
            if (category is null)
            {
                return NotFound();
            }

            if (!category.IsDeleted)
            {
                category.IsDeleted=true;
            }
            else
            {
                  _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}