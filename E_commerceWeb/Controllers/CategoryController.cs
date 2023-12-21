using E_commerceWeb.Data;
using E_commerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) 
        {
            _db = db;
        }
      
        public ViewResult Index(string sortOrder,string searchString)
        {

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DisplayOrderSortParm = sortOrder == "displayOrder" ? "displayOrder_desc" : "displayOrder";
            var objs = from obj in _db.Categories
                           select obj;
            if (!String.IsNullOrEmpty(searchString))
            {
                objs = objs.Where(obj => obj.Name.Contains(searchString));
                                       
            }
            switch (sortOrder)
            {
                case "name_desc":
                    objs = objs.OrderByDescending(obj => obj.Name);
                    break;
                case "displayOrder":
                    objs = objs.OrderBy(obj => obj.DisplayOrder);
                    break;
                case "displayOrder_desc":
                    objs = objs.OrderByDescending(obj => obj.DisplayOrder);
                    break;
                default:
                    objs = objs.OrderBy(obj => obj.Name);

                    break;
            }
            return View(objs.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        public IActionResult Create(Category obj)
        {   if(obj.Name == obj.DisplayOrder.ToString()) 
            {
                ModelState.AddModelError("name", "The Display Order can not exatly match the Category Name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfuly.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id== null || id== 0) 
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            if(categoryFromDb == null) 
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //POST
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order can not exatly match the Category Name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfuly.";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
           Category? obj = _db.Categories.Find(id);
            if (obj == null) 
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfuly.";
            return RedirectToAction("Index");
           
        }

    }   

            
}
