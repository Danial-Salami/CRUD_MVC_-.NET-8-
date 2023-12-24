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

        public ViewResult Index(string sortOrder, string searchString, int pg = 1)
        {
            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }
            List<Category> categories = _db.Categories.ToList();
            int recsCount = categories.Count();

           

            

            
            
            ViewBag.NameSortParm = sortOrder ==  "name" ? "name_desc" : "name";
            ViewBag.DisplayOrderSortParm = sortOrder == "displayOrder" ? "displayOrder_desc" : "displayOrder";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SearchString = searchString;
            var objs = from obj in _db.Categories
                           select obj;
            if (!String.IsNullOrEmpty(searchString))
            {
                 objs = objs.Where(obj => obj.Name.Contains(searchString));


                  recsCount = objs.Count(); 


            }
            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;

            switch (sortOrder)
            {
                case "name_desc":
                    objs = objs.OrderByDescending(obj => obj.Name);

                    break;
                case "name":
                    objs = objs.OrderBy(obj => obj.Name);

                    break;
                case "displayOrder":
                    objs = objs.OrderBy(obj => obj.DisplayOrder);
                    break;
                case "displayOrder_desc":
                    objs = objs.OrderByDescending(obj => obj.DisplayOrder);
                    break;
                default:
                    objs = objs.OrderBy(obj => obj.Id);

                    break;
            }
            var data = objs.Skip(recSkip).Take(pager.PageSize).ToList();
            return View(data);
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
