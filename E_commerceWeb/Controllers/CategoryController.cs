using E_commerceWeb.Data;
using E_commerceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace E_commerceWeb.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewResult Index(string sortOrder, string searchString, int pg = 1)
        {
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.DisplayOrderSortParm = sortOrder == "displayOrder" ? "displayOrder_desc" : "displayOrder";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SearchString = searchString;
            Pager pager;
            List<Category> data = _categoryService.GetCategoryList(sortOrder, searchString,out pager,pg);
            ViewBag.Pager = pager;
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        public IActionResult Create(Category obj, IFormFile? file)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order can not exatly match the Category Name");
            }
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string categoryPath = Path.Combine(wwwRootPath, @"images\category");
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\category\" + fileName;
                }
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfuly.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
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
        [HttpPost]
        public IActionResult Edit(Category obj, IFormFile? file)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order can not exatly match the Category Name");
            }
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string categoryPath = Path.Combine(wwwRootPath, @"images\category");
                    using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\category\" + fileName;
                }
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
