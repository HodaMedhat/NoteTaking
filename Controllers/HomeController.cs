using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NoteTaking.Models;
using NoteTaking.Services;
using NoteTaking.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace NoteTaking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly NoteService _note;
        private readonly UserService _user;


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, NoteService note, UserService user)
        {
            _logger = logger;
            _env = env;
            _note = note;
            _user = user;
        }
        //Home/Index?userId=1
        public IActionResult Index(decimal userId = 1)
        {
            try
            {
                var lst = _note.GetUserNotes(userId);
                ViewBag.UserId = userId;
                return View(lst);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/Index",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }

        public IActionResult AllIndex()
        {
            try
            {
                var lst = _note.GetAll();
                return View(lst);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/AllIndex",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }

        public IActionResult Trash(decimal userId = 1)
        {
            try
            {
                var lst = _note.GetUserTrash(userId);
                ViewBag.UserId = userId;
                return View(lst);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/Trash",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }
        //Home/Create?userId=1
        public IActionResult Create(decimal userId, decimal? noteId)
        {
            try
            {
                ViewBag.UserId = userId;
                NoteVM model = new NoteVM();
                if (noteId != null)
                {
                    model.Id = noteId.Value;
                    model = _note.Find(model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/Create",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult Create(NoteVM entity)
        {
            try
            {
                decimal err = 0;
                if (entity.Id == 0)
                {
                    err = _note.Add(entity);
                }
                else
                {
                    err = _note.Update(entity);
                }

                if (err == 1) return RedirectToAction("Index");
                else return RedirectToAction("Create");

            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/Create-Post",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }

        //[HttpPost]
        public IActionResult Delete(decimal id)
        {
            try
            {
                _note.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/Delete",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }

        public IActionResult Restore(decimal id)
        {
            try
            {
                _note.Restore(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "Home/Restore",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
