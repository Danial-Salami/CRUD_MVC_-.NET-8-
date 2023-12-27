using E_commerceWeb.Controllers;
using E_commerceWeb.Data;
using E_commerceWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;



namespace Services.Service;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
  
    public CategoryService(ApplicationDbContext dbContext,
        IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
       
    }

    public List<Category> GetCategoryList(string sortOrder, string searchString, out Pager pager, int pg = 1)
    {
        const int pageSize = 10;
        List<Category> categories = _dbContext.Categories.ToList();
        int recsCount = categories.Count;
        if (!string.IsNullOrEmpty(searchString))
        {
            categories = categories.Where(obj => obj.Name.Contains(searchString)).ToList();
            recsCount = categories.Count();
        }
        pager = new Pager(recsCount, pg, pageSize);
        if (pg < 1 && pg > pager.Endpage)
        {
            pg = 1;
        }
        int recSkip = (pg - 1) * pageSize;
        categories = sortOrder switch
        {
            "name_desc" => categories.OrderByDescending(obj => obj.Name).ToList(),
            "name" => categories.OrderBy(obj => obj.Name).ToList(),
            "displayOrder" => categories.OrderBy(obj => obj.DisplayOrder).ToList(),
            "displayOrder_desc" => categories.OrderByDescending(obj => obj.DisplayOrder).ToList(),
            _ => categories.OrderBy(obj => obj.Id).ToList(),
        };
        return categories.Skip(recSkip).Take(pager.PageSize).ToList();
    }
    public void CreateCategory(Category obj, IFormFile? file)
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

            _dbContext.Categories.Add(obj);
            _dbContext.SaveChanges();
            
        
    }
    public Category? GetCategoryById(int? id)
    {
        if (id == null || id == 0)
        {
            return null;
        }

        return _dbContext.Categories.Find(id);
    }

    public void EditCategory(Category obj, IFormFile? file)
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
        _dbContext.Categories.Update(obj);
        _dbContext.SaveChanges();
    }

    public void DeleteCategory(Category obj)
    {


       
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

            }
            _dbContext.Categories.Remove(obj);
            _dbContext.SaveChanges();

        
    }
}
