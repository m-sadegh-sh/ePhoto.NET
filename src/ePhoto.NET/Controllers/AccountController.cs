using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;

using ePhoto.NET.Helpers;
using ePhoto.NET.Models;

namespace ePhoto.NET.Controllers {
    [HandleError(View = "Error")]
    public partial class AccountController : Controller {
        private readonly IPhotoSharingContext _context;

        public AccountController(IPhotoSharingContext context) {
            _context = context;
        }

        [HttpGet]
        public virtual ActionResult Avatar(string slug, int size) {
            var user = _context.FindUserBySlug(slug);

            if (user == null)
                return HttpNotFound();

            var avatarFile = user.AvatarFile.ToResizedImage(width: size, height: size, compressionPercentage: 100);

            return File(avatarFile, user.AvatarMimeType);
        }

        [HttpGet]
        public virtual ActionResult Register() {
            return View(Views.Register);
        }

        [HttpPost]
        public virtual ActionResult Register(RegisterModel model) {
            if (ModelState.IsValid) {
                try {
                    _context.CreateUser(model.Email, model.Slug, model.Password, model.DisplayName);
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    return RedirectToAction(T4Routes.Photo.Index());
                } catch (DbValidationException exception) {
                    Logger.Write(exception);
                    ModelState.AddModelError("", exception.Message);
                } catch (Exception exception) {
                    Logger.Write(exception);
                    ModelState.AddModelError("", "لطفا مجددا تلاش کنید!");
                }
            }

            return View(Views.Register, model);
        }

        [HttpGet]
        public virtual ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;

            return View(Views.Login);
        }

        [HttpPost]
        public virtual ActionResult Login(LoginModel model, string returnUrl) {
            if (ModelState.IsValid) {
                if (_context.ValidateUser(model.Email, model.Password)) {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction(T4Routes.Photo.Index());
                }

                ModelState.AddModelError("", "ایمیل یا گذرواژه رو اشتباه وارد کردی!");
            }

            return View(Views.Login, model);
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult MyProfile() {
            var user = HttpContext.FindUser();

            var model = new MyProfileModel {Slug = user.Slug, DisplayName = user.DisplayName};

            return View(Views.MyProfile, model);
        }

        [HttpPost]
        [DbAuthorizer]
        public virtual ActionResult MyProfile(MyProfileModel model) {
            if (ModelState.IsValid) {
                try {
                    var user = HttpContext.FindUser();
                    user.Slug = model.Slug;
                    user.DisplayName = model.DisplayName;

                    if (model.AvatarFile != null) {
                        using (var stream = new BinaryReader(model.AvatarFile.InputStream))
                            user.AvatarFile = stream.ReadBytes(model.AvatarFile.ContentLength);
                        user.AvatarMimeType = model.AvatarFile.ContentType;
                    }

                    _context.SaveChanges();

                    return RedirectToAction(T4Routes.Photo.Index());
                } catch (DbValidationException exception) {
                    Logger.Write(exception);
                    ModelState.AddModelError("", exception.Message);
                } catch (Exception exception) {
                    Logger.Write(exception);
                    ModelState.AddModelError("", "لطفا مجددا تلاش کنید!");
                }
            }

            return View(Views.MyProfile, model);
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult ChangePassword() {
            return View(Views.ChangePassword);
        }

        [HttpPost]
        [DbAuthorizer]
        public virtual ActionResult ChangePassword(ChangePasswordModel model) {
            if (ModelState.IsValid) {
                try {
                    var user = HttpContext.FindUser();
                    _context.ChangeUserPassword(user, model.CurrentPassword, model.NewPassword);

                    return RedirectToAction(T4Routes.Photo.Index());
                } catch (DbValidationException exception) {
                    Logger.Write(exception);
                    ModelState.AddModelError("", exception.Message);
                } catch (Exception exception) {
                    Logger.Write(exception);
                    ModelState.AddModelError("", "لطفا مجددا تلاش کنید!");
                }
            }

            return View(Views.ChangePassword);
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult LogOff() {
            FormsAuthentication.SignOut();

            return RedirectToAction(T4Routes.Photo.Index());
        }
    }
}