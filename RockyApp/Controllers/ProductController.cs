using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RockyApp.Data;
using RockyApp.Models;
using RockyApp.Models.ViewModels;

namespace RockyApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db) 
        {
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;
            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(f => f.Id == obj.CategoryId);
            }
            return View(objList);
        }
        
        //GET - UpSert
        public IActionResult UpSert(int? id)
        {
            ProductVM productVm = new ProductVM()
            {
                Product = new Product(),
                CategoryDropDown = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            
            if (id == null)
            {
                return View(productVm);
            }
            else
            {
                productVm.Product = _db.Product.Find(id);
                if (productVm.Product == null) return NotFound();

                return View(productVm);
            }
        }
        
        // //POST - UpSert
        // [HttpPost]
        // [ValidateAntiForgeryToken] // Безопасность данных
        // public IActionResult UpSert(Product product)
        // {
        //
        //     // Проверка на то, что все ли правила валидации выполнены
        //     if (ModelState.IsValid)
        //     {
        //         _db.Product.Add(product);
        //         _db.SaveChanges();
        //         return RedirectToAction("Index");  // Перенаправляет  на Index
        //     }
        //
        //     return View(product);
        // }

        // //GET - Delete
        // public IActionResult Delete(int? id)
        // {
        //     // Проверка на то, что id не null и не равен 0
        //     if (id == null || id == 0) return NotFound();
        //
        //     // Поиск по первичному ключу
        //     var obj =  _db.Category.Find(id);
        //     if (obj == null) return NotFound();
        //     
        //     return View(obj);
        // }
        //
        // //POST - Delete
        // [HttpPost]
        // [ValidateAntiForgeryToken] // Безопасность данных
        // public IActionResult DeletePost(int? id)
        // {
        //     var obj = _db.Category.Find(id);
        //     if (obj == null)
        //     {
        //         return NotFound();
        //     }
        //     _db.Category.Remove(obj);
        //     _db.SaveChanges();
        //     return RedirectToAction("Index"); 
        // }
        
    }
}