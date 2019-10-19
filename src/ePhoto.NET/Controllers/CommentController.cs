using System;
using System.Linq;
using System.Web.Mvc;

using ePhoto.NET.Helpers;
using ePhoto.NET.Models;

namespace ePhoto.NET.Controllers {
    [HandleError(View = "Error")]
    public partial class CommentController : Controller {
        private readonly IPhotoSharingContext _context;

        public CommentController(IPhotoSharingContext context) {
            _context = context;
        }

        [HttpGet]
        [DbAuthorizer(AdminRequired = true)]
        public virtual ActionResult Manage() {
            var comments = _context.Comments.OrderByDescending(c => c.PostDate).ToList();

            return View(Views.Manage, comments);
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult New(string categorySlug, string photoSlug) {
            var photo = _context.FindPhotoBySlug(photoSlug);

            if (photo == null || !string.Equals(photo.Category.Slug, categorySlug, StringComparison.CurrentCulture))
                return HttpNotFound();

            var comment = new NewCommentModel {Photo = photo};

            return PartialView(Views.New, comment);
        }

        [HttpPost]
        [DbAuthorizer]
        public virtual ActionResult New(string categorySlug, string photoSlug, NewCommentModel model) {
            var photo = _context.FindPhotoBySlug(photoSlug);

            if (photo == null || !string.Equals(photo.Category.Slug, categorySlug, StringComparison.CurrentCulture))
                return HttpNotFound();

            var comment = new CommentModel {PhotoId = photo.PhotoId, UserId = HttpContext.FindUser().UserId, Body = model.Body, PostDate = DateTime.Now, IsApproved = HttpContext.FindUser().IsAdmin};

            _context.Add(comment);
            _context.SaveChanges();

            return RedirectToAction(T4Routes.Photo.Detail(photo.Category.Slug, photo.Slug));
        }

        [HttpGet]
        [DbAuthorizer(AdminRequired = true)]
        public virtual ActionResult Approve(int id) {
            var comment = _context.FindById<CommentModel>(id);

            if (comment == null)
                return HttpNotFound();

            comment.IsApproved = true;
            _context.SaveChanges();

            return RedirectToAction(T4Routes.Comment.Manage());
        }

        [HttpGet]
        [DbAuthorizer(AdminRequired = true)]
        public virtual ActionResult Delete(int id) {
            var comment = _context.FindById<CommentModel>(id);

            if (comment == null)
                return HttpNotFound();

            _context.Delete(comment);
            _context.SaveChanges();

            return RedirectToAction(T4Routes.Comment.Manage());
        }
    }
}