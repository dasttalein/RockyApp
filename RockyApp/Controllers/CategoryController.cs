using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RockyApp.Data;
using RockyApp.Models;

namespace RockyApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db) 
        {
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.Category;
            return View(objList);
        }
        
        //GET - Create
        public IActionResult Create()
        {
            IEnumerable<Category> objList = _db.Category;
            return View();
        }
        
        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult Create(Category category)
        {

            // Проверка на то, что все ли правила валидации выполнены
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");  // Перенаправляет  на Index
            }

            return View(category);
        }
        
        //GET - Edit
        public IActionResult Edit(int? id)
        {
            // Проверка на то, что id не null и не равен 0
            if (id == null || id == 0) return NotFound();

                // Поиск по первичному ключу
            var obj =  _db.Category.Find(id);
            if (obj == null) return NotFound();
            
            return View(obj);
        }
        
        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult Edit(Category category)
        {

            // Проверка на то, что все ли правила валидации выполнены
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }

            return View(category);
        }
        
        //GET - Delete
        public IActionResult Delete(int? id)
        {
            // Проверка на то, что id не null и не равен 0
            if (id == null || id == 0) return NotFound();

            // Поиск по первичному ключу
            var obj =  _db.Category.Find(id);
            if (obj == null) return NotFound();
            
            return View(obj);
        }
        
        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index"); 
        }
        
    }
}