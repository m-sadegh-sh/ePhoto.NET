using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

using ePhoto.NET.Helpers;
using ePhoto.NET.Models;

namespace ePhoto.NET.Controllers {
    [HandleError(View = "Error")]
    public partial class PhotoController : Controller {
        private readonly IPhotoSharingContext _context;

        public PhotoController(IPhotoSharingContext context) {
            _context = context;
        }

        [HttpGet]
        [DbAuthorizer(AdminRequired = true)]
        public virtual ActionResult Manage() {
            var photos = _context.Photos.OrderByDescending(p => p.CreationDate).ToList();

            return View(Views.Manage, photos);
        }

        [HttpGet]
        public virtual ActionResult IndexCategory(string slug) {
            var category = _context.FindCategoryBySlug(slug);

            if (category == null)
                return HttpNotFound();

            return View(Views.IndexCategory, category);
        }

        [HttpGet]
        public virtual ActionResult IndexUser(string slug) {
            var user = _context.FindUserBySlug(slug);

            if (user == null)
                return HttpNotFound();

            var model = new UserViewModel {User = user, PhotosCount = _context.FindPhotoCountsByUserId(user.UserId)};

            return View(Views.IndexUser, model);
        }

        [HttpGet]
        public virtual ActionResult IndexSearch(string queryText) {
            if (string.IsNullOrEmpty(queryText))
                return RedirectToAction(T4Routes.Photo.Index());

            return View(Views.IndexSearch, model: queryText);
        }

        [HttpGet]
        public virtual ActionResult Index() {
            return View(Views.Index);
        }

        [HttpGet]
        public virtual ActionResult ListCategory(string slug, int page, int size) {
            var photos = _context.FindPhotosByCategorySlug(slug, page, size);

            var model = MapToViewModel(photos);

            return PartialView(Views.List, model);
        }

        [HttpGet]
        public virtual ActionResult ListUser(string slug, int page, int size) {
            var photos = _context.FindPhotosByUserSlug(slug, page, size);

            var model = MapToViewModel(photos);

            return PartialView(Views.List, model);
        }

        [HttpGet]
        public virtual ActionResult ListSearch(string queryText, int page, int size) {
            var photos = _context.FindPhotosByQuery(queryText, page, size);
            
            var model = MapToViewModel(photos);

            return PartialView(Views.List, model);
        }

        [HttpGet]
        public virtual ActionResult List(int page, int size) {
            var photos = _context.FindPhotos(page, size);

            var model = MapToViewModel(photos);

            return PartialView(Views.List, model);
        }

        [HttpGet]
        public virtual ActionResult Detail(string categorySlug, string photoSlug) {
            var photo = _context.FindPhotoBySlug(photoSlug);

            if (photo == null || !string.Equals(photo.Category.Slug, categorySlug, StringComparison.CurrentCulture))
                return HttpNotFound();

            if (!photo.IsApproved && (HttpContext.FindUser().UserId != photo.UserId || HttpContext.FindUser().IsAdmin))
                return HttpNotFound();

            _context.CreateUserPhotoAction(HttpContext.FindUser(false)?.UserId, photo.PhotoId, UserPhotoActionType.View, false);

            var model = new PhotoDetailViewModel {Photo = photo, PhotosCount = _context.FindPhotoCountsByUserId(photo.User.UserId), ViewsCount = _context.FindUserPhotoActionsCountByType(photo.PhotoId, UserPhotoActionType.View), LikesCount = _context.FindUserPhotoActionsCountByType(photo.PhotoId, UserPhotoActionType.Like), CommentsCount = _context.FindCommentsCountByPhotoId(photo.PhotoId), DownloadsCount = _context.FindUserPhotoActionsCountByType(photo.PhotoId, UserPhotoActionType.Download), IsLiked = Request.IsAuthenticated && _context.HasUserPhotoAction(HttpContext.FindUser().UserId, photo.PhotoId, UserPhotoActionType.Like), LikedUsers = _context.FindUserPhotoActionsByType(photo.PhotoId, UserPhotoActionType.Like), Comments = _context.FindCommentsByPhotoId(photo.PhotoId, 1, int.MaxValue)};

            return View(Views.Detail, model);
        }

        [HttpGet]
        public virtual ActionResult Download(string categorySlug, string photoSlug, int? width = null, int? height = null) {
            var photo = _context.FindPhotoBySlug(photoSlug);

            if (photo == null || !string.Equals(photo.Category.Slug, categorySlug, StringComparison.CurrentCulture))
                return HttpNotFound();

            byte[] file;

            if (width == null) {
                file = photo.PhotoFile;
                _context.CreateUserPhotoAction(HttpContext.FindUser(false)?.UserId, photo.PhotoId, UserPhotoActionType.Download, false);
            } else
                file = photo.PhotoFile.ToResizedImage(width: (int) width, height: height.GetValueOrDefault((int) width), compressionPercentage: 80);

            return File(file, photo.PhotoMimeType, $"ephoto-net-{photo.Slug}.jpg");
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult New() {
            var model = new NewPhotoModel {Categories = _context.GetCategoriesList()};

            return View(Views.New, model);
        }

        [HttpPost]
        [DbAuthorizer]
        public virtual ActionResult New(NewPhotoModel model) {
            model.Categories = _context.GetCategoriesList(model.CategoryId);

            if (!ModelState.IsValid)
                return View(Views.New, model);

            var photo = _context.Photos.Create();

            photo.Title = model.Title;
            photo.Slug = model.Slug;
            photo.Description = model.Description;
            photo.CreationDate = DateTime.Now;
            photo.UserId = HttpContext.FindUser().UserId;
            photo.CategoryId = model.CategoryId;
            photo.PhotoMimeType = model.PhotoFile.ContentType;
            photo.IsApproved = HttpContext.FindUser().IsAdmin;

            using (var stream = new BinaryReader(model.PhotoFile.InputStream))
                photo.PhotoFile = stream.ReadBytes(model.PhotoFile.ContentLength);

            _context.Add(photo);
            _context.SaveChanges();

            return RedirectToAction(T4Routes.Photo.Detail(photo.Category.Slug, photo.Slug));
        }

        [HttpGet]
        [DbAuthorizer(AdminRequired = true)]
        public virtual ActionResult Approve(int id) {
            var photo = _context.FindById<PhotoModel>(id);

            if (photo == null)
                return HttpNotFound();

            photo.IsApproved = true;
            _context.SaveChanges();

            return RedirectToAction(T4Routes.Photo.Manage());
        }

        [HttpGet]
        [DbAuthorizer(AdminRequired = true)]
        public virtual ActionResult Delete(int id) {
            var photo = _context.FindById<PhotoModel>(id);

            if (photo == null)
                return HttpNotFound();

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

            return RedirectToAction(T4Routes.Photo.Manage());
        }

        [HttpGet]
        [DbAuthorizer]
        public virtual ActionResult Like(string categorySlug, string photoSlug) {
            var photo = _context.FindPhotoBySlug(photoSlug);

            if (photo == null || !string.Equals(photo.Category.Slug, categorySlug, StringComparison.CurrentCulture))
                return HttpNotFound();

            _context.CreateUserPhotoAction(HttpContext.FindUser().UserId, photo.PhotoId, UserPhotoActionType.Like, true);

            return RedirectToAction(T4Routes.Photo.Detail(photo.Category.Slug, photo.Slug));
        }

        private List<PhotoViewModel> MapToViewModel(IEnumerable<PhotoModel> photos) {
            return photos.Select(p => new PhotoViewModel {Photo = p, ViewsCount = _context.FindUserPhotoActionsCountByType(p.PhotoId, UserPhotoActionType.View), LikesCount = _context.FindUserPhotoActionsCountByType(p.PhotoId, UserPhotoActionType.Like), CommentsCount = _context.FindCommentsCountByPhotoId(p.PhotoId)}).ToList();
        }
    }
}