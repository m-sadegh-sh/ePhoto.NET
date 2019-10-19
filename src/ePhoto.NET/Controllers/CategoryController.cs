using System.Linq;
using System.Web.Mvc;

using ePhoto.NET.Helpers;
using ePhoto.NET.Models;

namespace ePhoto.NET.Controllers {
    [HandleError(View = "Error")]
    [DbAuthorizer(AdminRequired = true)]
    public partial class CategoryController : Controller {
        private readonly IPhotoSharingContext _context;

        public CategoryController(IPhotoSharingContext context) {
            _context = context;
        }

        [HttpGet]
        public virtual ActionResult Manage() {
            var categories = _context.Categories.OrderByDescending(c => c.Name).ToList();

            return View(Views.Manage, categories);
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult NewOrEdit(int id) {
            var model = new CategoryModel();

            if (id != default(int)) {
                model = _context.FindById<CategoryModel>(id);

                if (model == null)
                    return HttpNotFound();
            }

            return View(Views.NewOrEdit, model);
        }

        [HttpPost]
        [DbAuthorizer]
        public virtual ActionResult NewOrEdit(CategoryModel model) {
            if (!ModelState.IsValid)
                return View(Views.NewOrEdit, model);

            var isNew = model.CategoryId == default(int);
            var category = isNew ? _context.Categories.Create() : _context.FindById<CategoryModel>(model.CategoryId);

            category.Name = model.Name;
            category.Slug = model.Slug;

            if (isNew)
                _context.Add(category);

            _context.SaveChanges();

            return RedirectToAction(T4Routes.Category.Manage());
        }

        [HttpGet]
        public virtual ActionResult Delete(int id) {
            var category = _context.FindById<CategoryModel>(id);

            if (category == null)
                return HttpNotFound();

            var photos = _context.FindPhotosByCategorySlug(category.Slug, 1, int.MaxValue);

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

            _context.Delete(category);
            _context.SaveChanges();

            return RedirectToAction(T4Routes.Category.Manage());
        }
    }
}