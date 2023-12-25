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
}
