using System.Linq;
using System.Web.Mvc;

using ePhoto.NET.Helpers;
using ePhoto.NET.Models;

namespace ePhoto.NET.Controllers {
    [HandleError(View = "Error")]
    [DbAuthorizer(AdminRequired = true)]
    public partial class UserController : Controller {
        private readonly IPhotoSharingContext _context;

        public UserController(IPhotoSharingContext context) {
            _context = context;
        }

        [HttpGet]
        public virtual ActionResult Manage() {
            var users = _context.Users.OrderByDescending(u => u.DisplayName).ToList();

            return View(Views.Manage, users);
        }

        [HttpGet]
        public virtual ActionResult MarkAsNormalUser(int id) {
            var user = _context.FindById<UserModel>(id);

            if (user == null)
                return HttpNotFound();

            user.IsAdmin = false;

            _context.SaveChanges();

            return RedirectToAction(T4Routes.User.Manage());
        }

        [HttpGet]
        public virtual ActionResult MarkAsAdmin(int id) {
            var user = _context.FindById<UserModel>(id);

            if (user == null)
                return HttpNotFound();

            user.IsAdmin = true;

            _context.SaveChanges();

            return RedirectToAction(T4Routes.User.Manage());
        }

        [HttpGet]
        public virtual ActionResult Delete(int id) {
            var user = _context.FindById<UserModel>(id);

            if (user == null)
                return HttpNotFound();

            var photos = _context.FindPhotosByUserSlug(user.Slug, 1, int.MaxValue);

            foreach (var photo in photos) {
                var userPhotoActions = _context.FindUserPhotoActionsByPhotoId(photo.PhotoId);
                var comments = _context.FindCommentsByPhotoId(photo.PhotoId);

                foreach (var userPhotoAction in userPhotoActions) {
                    _context.Delete(userPhotoAction);
                    _context.SaveChanges();
                }

                foreach (var comment in comments) {
                    _context.Delete(comment);
                    _context.SaveChanges();
                }

                _context.Delete(photo);
                _context.SaveChanges();
            }

            _context.Delete(user);
            _context.SaveChanges();

            return RedirectToAction(T4Routes.User.Manage());
        }
    }
}