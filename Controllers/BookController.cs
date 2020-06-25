using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
  public class BookController : Controller
  {
    private readonly ApplicationDbContext _db;

    public BookController(ApplicationDbContext db)
    {
      _db = db;
    }

    [BindProperty] public Book Book { get; set; }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Upsert(int? id)
    {
      if (id == null)
      {
        Book = new Book();
        return View(Book);
      }

      Book = _db.Book.Find(id);
      if (Book == null)
      {
        return NotFound();
      }

      return View(Book);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert()
    {
      if (ModelState.IsValid)
      {
        if (Book.Id == 0)
        {
          _db.Book.Add(Book);
        }
        else
        {
          _db.Book.Update(Book);
        }

        _db.SaveChanges();

        return RedirectToAction("Index");
      }
      return View(Book);
    }


    #region API Calls
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
      return Json(new
      {
        data = await _db.Book.ToListAsync()
      });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBook(int id)
    {
      var bookFromDb = await _db.Book.FindAsync(id);

      if (bookFromDb == null)
      {
        return Json((new
        {
          success = false,
          message = "Error while deleting book."
        }));
      }

      _db.Book.Remove(bookFromDb);
      await _db.SaveChangesAsync();

      return Json(new
      {
        success = true,
        message = "Delete successful."
      });
    }
    #endregion
  }
}