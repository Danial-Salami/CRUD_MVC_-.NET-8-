using E_commerceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService;

public interface ICategoryService
{
    List<Category> GetCategoryList(string sortOrder, string searchString, out Pager pager, int pg = 1);
    void CreateCategory(Category obj, IFormFile? file);
    Category? GetCategoryById(int? id);
    void EditCategory(Category obj, IFormFile? file);
    public void DeleteCategory(Category obj);
}
