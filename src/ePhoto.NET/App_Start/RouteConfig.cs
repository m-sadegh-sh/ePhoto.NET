using System.Web.Mvc;
using System.Web.Routing;

using ePhoto.NET.Helpers;

namespace ePhoto.NET {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("HomeAbout", "about", T4Routes.Home.About());
            routes.MapRoute("HomeContact", "contact", T4Routes.Home.Contact());

            routes.MapRoute("AccountAvatar", "{slug}/avatar-{size}", T4Routes.Account.Avatar(), null, new {slug = Patterns.Slug, size = Patterns.AnyNumber});
            routes.MapRoute("AccountRegister", "register", T4Routes.Account.Register());
            routes.MapRoute("AccountLogin", "login", T4Routes.Account.Login());
            routes.MapRoute("AccountMyProfile", "my-profile", T4Routes.Account.MyProfile());
            routes.MapRoute("AccountChangePassword", "change-password", T4Routes.Account.ChangePassword());
            routes.MapRoute("AccountLogOff", "logoff", T4Routes.Account.LogOff());

            routes.MapRoute("PhotoListCategory", "categories/{slug}/{page}x{size}", T4Routes.Photo.ListCategory(), null, new {slug = Patterns.Slug, page = Patterns.AnyNumber, size = Patterns.AnyNumber});
            routes.MapRoute("PhotoListSearch", "search/{page}x{size}", T4Routes.Photo.ListSearch(), null, new {page = Patterns.AnyNumber, size = Patterns.AnyNumber});
            routes.MapRoute("PhotoListUser", "{slug}/{page}x{size}", T4Routes.Photo.ListUser(), null, new {slug = Patterns.Slug, page = Patterns.AnyNumber, size = Patterns.AnyNumber});
            routes.MapRoute("PhotoList", "list/{page}x{size}", T4Routes.Photo.List(), null, new {page = Patterns.AnyNumber, size = Patterns.AnyNumber});

            routes.MapRoute("PhotoIndexCategory", "categories/{slug}", T4Routes.Photo.IndexCategory(), null, new {slug = Patterns.Slug});
            routes.MapRoute("PhotoSearch", "search/", T4Routes.Photo.IndexSearch());

            routes.MapRoute("CategoryManage", "management/categories", T4Routes.Category.Manage());
            routes.MapRoute("CategoryNewOrEdit", "management/categories/new-or-edit/{id}", T4Routes.Category.NewOrEdit(), null, new { id = Patterns.AnyNumber });
            routes.MapRoute("CategoryDelete", "management/categories/delete/{id}", T4Routes.Category.Delete(), null, new { id = Patterns.AnyNumber });
            routes.MapRoute("UserManage", "management/users", T4Routes.User.Manage());
            routes.MapRoute("UserMarkAsNormalUser", "management/users/mark-as-normal-user/{id}", T4Routes.User.MarkAsNormalUser(), null, new { id = Patterns.AnyNumber });
            routes.MapRoute("UserMarkAsAdmin", "management/users/mark-as-admin/{id}", T4Routes.User.MarkAsAdmin(), null, new { id = Patterns.AnyNumber });
            routes.MapRoute("UserDelete", "management/users/delete/{id}", T4Routes.User.Delete(), null, new { id = Patterns.AnyNumber });
            routes.MapRoute("PhotoManage", "management/photos", T4Routes.Photo.Manage());
            routes.MapRoute("CommentManage", "management/comments", T4Routes.Comment.Manage());
            routes.MapRoute("PhotoNew", "photo/new", T4Routes.Photo.New());
            routes.MapRoute("PhotoApprove", "management/photos/approve/{id}", T4Routes.Photo.Approve(), null, new {id = Patterns.AnyNumber});
            routes.MapRoute("PhotoDelete", "management/photos/delete/{id}", T4Routes.Photo.Delete(), null, new {id = Patterns.AnyNumber});
            routes.MapRoute("PhotoDetail", "{categorySlug}/{photoSlug}", T4Routes.Photo.Detail(), null, new {categorySlug = Patterns.Slug, photoSlug = Patterns.Slug});
            routes.MapRoute("PhotoDownloadWidthHeight", "{categorySlug}/{photoSlug}/download-{width}x{height}", T4Routes.Photo.Download(), null, new {categorySlug = Patterns.Slug, photoSlug = Patterns.Slug, width = Patterns.AnyNumber, height = Patterns.AnyNumber});
            routes.MapRoute("PhotoDownloadWidth", "{categorySlug}/{photoSlug}/download-{width}", T4Routes.Photo.Download(), null, new {categorySlug = Patterns.Slug, photoSlug = Patterns.Slug, width = Patterns.AnyNumber});
            routes.MapRoute("PhotoDownload", "photo/{categorySlug}/{photoSlug}/download", T4Routes.Photo.Download(), null, new {categorySlug = Patterns.Slug, photoSlug = Patterns.Slug});
            routes.MapRoute("PhotoLike", "photo/{categorySlug}/{photoSlug}/like", T4Routes.Photo.Like(), null, new {categorySlug = Patterns.Slug, photoSlug = Patterns.Slug});

            routes.MapRoute("CommentNew", "photo/{categorySlug}/{photoSlug}/comments/new", T4Routes.Comment.New(), null, new {categorySlug = Patterns.Slug, photoSlug = Patterns.Slug});
            routes.MapRoute("CommentApprove", "management/comments/approve/{id}", T4Routes.Comment.Approve(), null, new {id = Patterns.AnyNumber});
            routes.MapRoute("CommentDelete", "management/comments/delete/{id}", T4Routes.Comment.Delete(), null, new {id = Patterns.AnyNumber});

            routes.MapRoute("PhotoIndexUser", "{slug}", T4Routes.Photo.IndexUser(), null, new {slug = Patterns.Slug});
            routes.MapRoute("PhotoIndex", "", T4Routes.Photo.Index());
        }
    }
}