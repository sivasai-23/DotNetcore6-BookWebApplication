using BookListWebApp.Data;
using BookListWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookListWebApp.Controllers
{
    public class CategoryController : Controller
    {
        #region for Getting DB context from program.cs dependency injection container
        private readonly ApplicationDbContext _db;
        //constructor for Category Controller
        public CategoryController(ApplicationDbContext DB)
        {
            _db = DB;
        }
        #endregion
        #region for Get all categories from DB while onloading time
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;
            return View(objCategoryList);
        }
        #endregion
        #region for Add new Category
        //Get
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("cusNameDisOrdMatch", "The display order can not match the name.");
            }
            if (ModelState.IsValid)
            {

                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");

            }
            return View(obj);
        }

        #endregion
        #region for Edit Category
        //Get
        public IActionResult EditCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var catergoryFromDB = _db.Categories.Find(id);
            //var catergoryFromDBFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            //var catergoryFromDBSingOrDef = _db.Categories.FirstOrDefault(c => c.Id == id);
            if(catergoryFromDB == null)
            {
                return NotFound();
            }
            return View(catergoryFromDB);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order can not match the name.");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View(obj);
        }
        #endregion

        #region for Delete Category
        //Get
        public IActionResult DeleteCategory(int? id)
        {
            if(id == null || id == 0)
            {
                NotFound();
            }
            var delCategFromDb = _db.Categories.Find(id);
            if(delCategFromDb == null)
            {
                NotFound();
            }
            return View(delCategFromDb);
        }
        //post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryPost(int? id)
        {
            var objDel = _db.Categories.Find(id);
           if(objDel == null)
            {
                NotFound();
            }
           if(objDel != null)
            {
                _db.Categories.Remove(objDel);
                _db.SaveChanges();
               TempData["success"]  = "Category deleted successfully";
                return RedirectToAction("Index", "Category");
            }

            return RedirectToAction("Index", "Category");
        }
        #endregion
    }
}
