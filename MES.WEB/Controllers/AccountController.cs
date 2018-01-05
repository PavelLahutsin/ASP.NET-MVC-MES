using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Interfaces;
using MES.WEB.Models;
using Microsoft.Owin.Security;

namespace MES.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(
            IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        //private IUserService UserService => HttpContext.GetOwinContext().GetUserManager<IUserService>();

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVm model)
        {
            if (!ModelState.IsValid) return View(model);
            var userDto = Mapper.Map<UserDTO>(model);
            ClaimsIdentity claim = await _userService.Authenticate(userDto);
            if (claim == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
            }
            else
            {
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Register(RegisterVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var userDto = Mapper.Map<UserDTO>(model);
            var operationDetails = await _userService.Create(userDto);
            if (operationDetails.Succedeed)
                return RedirectToAction("Index", "Home");
            else
                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            return View(model);
        }

    }
}