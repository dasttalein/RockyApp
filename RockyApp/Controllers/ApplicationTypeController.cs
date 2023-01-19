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
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db) 
        {
            _db = db;
        }

        // GET
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _db.ApplicationType;
            return View(objList);
        }
        
        //GET - Create
        public IActionResult Create()
        {
            IEnumerable<ApplicationType> objList = _db.ApplicationType;
            return View();
        }
        
        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult Create(ApplicationType applicationType)
        {
            _db.ApplicationType.Add(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        //GET - Edit
        public IActionResult Edit(int? id)
        {
            // Проверка на то, что id не null и не равен 0
            if (id == null || id == 0) return NotFound();

            // Поиск по первичному ключу
            var obj =  _db.ApplicationType.Find(id);
            if (obj == null) return NotFound();
            
            return View(obj);
        }
        
        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult Edit(ApplicationType applicationType)
        {

            // Проверка на то, что все ли правила валидации выполнены
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(applicationType);
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }

            return View(applicationType);
        }
        
        //GET - Delete
        public IActionResult Delete(int? id)
        {
            // Проверка на то, что id не null и не равен 0
            if (id == null || id == 0) return NotFound();

            // Поиск по первичному ключу
            var obj =  _db.ApplicationType.Find(id);
            if (obj == null) return NotFound();
            
            return View(obj);
        }
        
        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken] // Безопасность данных
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.ApplicationType.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.ApplicationType.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index"); 
        }
    }
}