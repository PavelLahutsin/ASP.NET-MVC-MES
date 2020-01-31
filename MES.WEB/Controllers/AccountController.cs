using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MES.BLL.DTO;
using MES.BLL.Infrastructure;
using MES.BLL.Interfaces;
using MES.DAL.Interfaces;
using MES.WEB.Models;
using Microsoft.Owin.Security;

namespace MES.WEB.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUnitOfWork _serviceOfWork;
        private readonly IUserService _service;

        public AccountController(IUnitOfWork serviceOfWork, IUserService service)
        {
            _serviceOfWork = serviceOfWork;
            this._service = service;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVm model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _serviceOfWork.Users.Entities.FirstOrDefaultAsync(u =>
                u.UserName == model.UserName && u.Password == model.Password && !u.IsDeleted);

            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль.");
            }
            else
            {
                ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
                claim.AddClaim(new Claim(
                    "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "OWIN Provider", ClaimValueTypes.String));
                if (user.Role != null)
                    claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name,
                        ClaimValueTypes.String));

                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterVm modal, HttpPostedFileBase imageUpload = null)
        {
            if (!ModelState.IsValid) return View(modal);
            if (imageUpload != null)
            {
                var count = imageUpload.ContentLength;
                modal.Image = new byte[count];
                imageUpload.InputStream.Read(modal.Image, 0, count);
                modal.MimeType = imageUpload.ContentType;
            }
            var result = await _service.Register(Mapper.Map<UserDto>(modal));
            return View("SuccessRegister", result);
        }
        [Authorize]     
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<FileResult> GetImage(int id)
        {
            var user = await _serviceOfWork.Users.GetAsync(id);
            return user != null ? File(user.Image, user.MimeType) : null;
        }

        [Authorize]
        public async Task<ActionResult> Edit()
        {
            var id = User.Identity.GetUserId<int>();
            var user = Mapper.Map<RegisterVm>(await _serviceOfWork.Users.GetAsync(id));
            return View(user);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]        
        public async Task<ActionResult> Edit(RegisterVm modal, HttpPostedFileBase imageUpload = null)
        {
            if (!ModelState.IsValid) return View(modal);
            if (imageUpload != null)
            {
                var count = imageUpload.ContentLength;
                modal.Image = new byte[count];
                imageUpload.InputStream.Read(modal.Image, 0, count);
                modal.MimeType = imageUpload.ContentType;
            }
            var result = await _service.EditUser(Mapper.Map<UserDto>(modal));
            if (result.Succedeed) Logout();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public  ActionResult ShowProfile()
        {
            var id = User.Identity.GetUserId<int>();
            var image = _serviceOfWork.Users.Entities.Where(w=>w.Id==id).Select(x => x.Image).FirstOrDefault();
            
            return PartialView(image);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteUser(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public async Task<ActionResult> GetUsers(string startDate, string endDate)
        {
            var users = Mapper.Map<IEnumerable<UserDto>, IEnumerable<ChatUser>>(
                await _service.GetUsers());
            return PartialView(users);
        }
    }
}