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
                var errorMessage = "The Display Order cannot exactly match the Category Name";
                ModelState.AddModelError("name", errorMessage);
                throw new InvalidOperationException(errorMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.CreateCategory(obj, file);
                    TempData["success"] = "Category created successfully.";
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("name", ex.Message);
                    return View();
                }
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            Category? categoryFromDb = _categoryService.GetCategoryById(id);

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
                
                _categoryService.EditCategory(obj, file);
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
            Category? categoryFromDb = _categoryService.GetCategoryById(id);
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
            Category? obj = _categoryService.GetCategoryById(id);
            if (obj == null)
            {
                return NotFound();
            }
            _categoryService.DeleteCategory(obj);
            TempData["success"] = "Category deleted successfuly.";
            return RedirectToAction("Index");

        }

    }


}
