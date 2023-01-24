using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RockyApp.Data;
using RockyApp.Models;
using RockyApp.Models.ViewModels;

namespace RockyApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment) 
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
        
        //POST - UpSert
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult UpSert(ProductVM productVm)
        {
            // Проверка на то, что все ли правила валидации выполнены
            if (ModelState.IsValid)
            {
                // Получаем загруженное изображение
                var files = HttpContext.Request.Form.Files;
                //  Получаем доступ к папке wwwroot
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVm.Product.Id == 0)
                {
                    // Creating
                    string upload = webRootPath + WC.ImagePath; // Получаем путь в папку, где будут хрнаиться изображения продуктов
                    string filename = Guid.NewGuid().ToString(); // Имя файла (Guid)
                    string extension = Path.GetExtension(files[0].FileName); // Значение из файла, который был загружен

                    // Копируем файл в новое место
                    using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVm.Product.Image = filename + extension;
                    
                    // Add new product
                    _db.Product.Add(productVm.Product);
                }
                else
                {
                    // Updating
                    // AsNoTracking - Entity Framework не отслеживает этот объект
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(f => f.Id == productVm.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath; // Получаем путь в папку, где будут хрнаиться изображения продуктов
                        string filename = Guid.NewGuid().ToString(); // Имя файла (Guid)
                        string extension = Path.GetExtension(files[0].FileName); // Значение из файла, который был загружен

                        var oldFile = Path.Combine(upload, objFromDb?.Image);

                        // Удаление старого фото
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        
                        using (var fileStream = new FileStream(Path.Combine(upload, filename + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVm.Product.Image = filename + extension;
                    }
                    else
                    {
                        productVm.Product.Image = objFromDb?.Image;
                    }

                    _db.Product.Update(productVm.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            // Если вдруг будет ошибка валидации, то восстанавливаем то, что может пропасть
            productVm.CategoryDropDown = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVm);
        }

        //GET - Delete
        public IActionResult Delete(int? id)
        {
            // Проверка на то, что id не null и не равен 0
            if (id == null || id == 0) return NotFound();
        
            // Используем жадную загрузку (Вместе с продуктом мы сразу получаем и категорию)
            Product product = _db.Product.Include(i => i.Category).FirstOrDefault(f => f.Id == id);
            if (product == null) return NotFound();
            
            return View(product);
        }
        
        //POST - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath; // Получаем путь в папку, где будут хрнаиться изображения продуктов
            var oldFile = Path.Combine(upload, obj?.Image);

            // Удаление старого фото
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}