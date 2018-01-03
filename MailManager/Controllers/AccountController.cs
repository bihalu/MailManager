using CryptSharp;
using MailManager.Data;
using MailManager.Models;
using MailManager.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace MailManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;

        private readonly IUserManager _userManager;

        public AccountController(DataContext dataContext, IUserManager userManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Info()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginViewModel login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                login.Message = "Benutzer und Kennwort eingeben";
                return View(login);
            }

            var account = _userManager.Validate(login.Username, login.Password);

            if (null == account)
            {
                login.Message = "Zugriff verweigert!";
                return View(login);
            }
            _userManager.SignIn(HttpContext, account, false);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            _userManager.SignOut(HttpContext);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var accounts = new List<Account>();

            if (_dataContext.Accounts.Any())
            {
                accounts = _dataContext.Accounts.ToList();
            }

            return View(accounts);
        }

        [Authorize(Policy = "IsAdmin")]
        public IActionResult Edit([FromQuery] int id)
        {
            var account = _dataContext.Accounts.Find(id);

            if (null == account)
            {
                return RedirectToAction("Index", "Account");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Policy = "IsAdmin")]
        public IActionResult Lock([FromQuery] int id)
        {
            var account = _dataContext.Accounts.Find(id);

            if (null != account)
            {
                account.Enabled = false;
                _dataContext.SaveChanges();
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        [Authorize(Policy = "IsAdmin")]
        public IActionResult Unlock([FromQuery] int id)
        {
            var account = _dataContext.Accounts.Find(id);

            if (null != account)
            {
                account.Enabled = true;
                _dataContext.SaveChanges();
            }

            return RedirectToAction("Index", "Account");
        }

        [Authorize]
        public IActionResult ResetPassword([FromQuery] int id, [FromForm] LoginViewModel login)
        {
            var account = _dataContext.Accounts.Find(id);

            if(null == account)
            {
                return RedirectToAction("Index", "Account");
            }

            if(User.Identity.Name == account.Username || User.IsInRole("Admin"))
            { 
                if (login.Id == 0)
                {
                    var model = new LoginViewModel()
                    {
                        Id = account.Id,
                        Username = account.Username,
                        Password = string.Empty,
                        ConfirmPassword = string.Empty
                    };

                    return View(model);
                }
                else
                {
                    if(login.Password == login.ConfirmPassword)
                    {
                        if(_userManager.WeakPassword(login.Password))
                        {
                            login.Message = "Das Kennwort ist zu schwach!";
                            return View(login);
                        }
                        var passwordHash = Crypter.Sha512.Crypt(login.Password);
                        account.Password = passwordHash;
                        _dataContext.SaveChanges();
                    }
                    else
                    {
                        login.Message = "Kennwörter stimmen nicht überein!";
                        return View(login);
                    }
                }

                return RedirectToAction("Index", "Account");
            }

            return RedirectToAction("AccessDenied", "Account");
        }
    }
}
