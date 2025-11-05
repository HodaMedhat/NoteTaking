using Microsoft.AspNetCore.Mvc;
using NoteTaking.Models;
using NoteTaking.Services;
using NoteTaking.ViewModel;

namespace NoteTaking.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly NoteService _note;
        private readonly UserService _user;

        public UserController(ILogger<HomeController> logger, IWebHostEnvironment env, NoteService note, UserService user)
        {
            _logger = logger;
            _env = env;
            _note = note;
            _user = user;
        }
       
       //User/Login
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                var logger = new ErrorLogger(_env);
                logger.AppendErrors(new List<ErrorViewModel> { new ErrorViewModel {
                FieldName = "User/Login",
                ErrorMessage = ex.Message,
                ErrorMessageInner = ex.InnerException != null? ex.InnerException.Message:"0" }
                 });
                return RedirectToAction("Error");
            }
        }
    }
}
