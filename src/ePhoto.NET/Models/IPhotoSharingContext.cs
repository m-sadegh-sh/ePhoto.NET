using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;

namespace ePhoto.NET.Models {
    public interface IPhotoSharingContext {
        DbSet<UserModel> Users { get; }
        DbSet<CategoryModel> Categories { get; }
        DbSet<PhotoModel> Photos { get; }
        DbSet<CommentModel> Comments { get; }
        int SaveChanges();
        T Add<T>(T entity) where T : class;
        T FindById<T>(int id) where T : class;
        T Delete<T>(T entity) where T : class;
        UserModel FindUserByEmail(string email);
        UserModel FindUserBySlug(string slug);
        CategoryModel FindCategoryBySlug(string slug);
        PhotoModel FindPhotoBySlug(string slug);
        List<PhotoModel> FindPhotosByCategorySlug(string slug, int page, int size);
        List<PhotoModel> FindPhotosByUserSlug(string slug, int page, int size);
        List<PhotoModel> FindPhotosByQuery(string queryText, int page, int size);
        List<PhotoModel> FindPhotos(int page, int size);
        int FindPhotoCountsByCategoryId(int categoryId);
        int FindPhotoCountsByUserId(int userId);
        void CreateUser(string email, string slug, string password, string displayName);
        bool ValidateUser(string email, string password);
        void ChangeUserPassword(UserModel user, string currentPassword, string newPassword);
        IDictionary<CategoryModel, int> FindCategoriesCloud();
        int FindPhotosCount();
        List<CommentModel> FindCommentsByPhotoId(int photoId);
        List<CommentModel> FindCommentsByUserId(int userId);
        List<CommentModel> FindCommentsByPhotoId(int photoId, int page, int size);
        int FindCommentsCountByPhotoId(int photoId);
        List<UserPhotoActionModel> FindUserPhotoActionsByPhotoId(int photoId);
        List<UserPhotoActionModel> FindUserPhotoActionsByUserId(int userId);
        List<UserModel> FindUserPhotoActionsByType(int photoId, UserPhotoActionType actionType);
        int FindUserPhotoActionsCountByType(int photoId, UserPhotoActionType actionType);
        bool HasUserPhotoAction(int userId, int photoId, UserPhotoActionType actionType);
        void CreateUserPhotoAction(int? userId, int photoId, UserPhotoActionType actionType, bool preventDuplicate);
        IEnumerable<SelectListItem> GetCategoriesList(int? selectedCategoryId = null);
    }
}