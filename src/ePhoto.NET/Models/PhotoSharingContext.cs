using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ePhoto.NET.Models {
    public class PhotoSharingContext : DbContext, IPhotoSharingContext {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<PhotoModel> Photos { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<UserPhotoActionModel> UserPhotoActions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PhotoModel>().HasRequired(p => p.User).WithMany().WillCascadeOnDelete(false);
        }

        public T Add<T>(T entity) where T : class {
            return Set<T>().Add(entity);
        }

        public T FindById<T>(int id) where T : class {
            return Set<T>().Find(id);
        }

        public T Delete<T>(T entity) where T : class {
            return Set<T>().Remove(entity);
        }

        public UserModel FindUserByEmail(string email) {
            return Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        public UserModel FindUserBySlug(string slug) {
            return Users.FirstOrDefault(u => u.Slug.Equals(slug, StringComparison.CurrentCultureIgnoreCase));
        }

        public CategoryModel FindCategoryBySlug(string slug) {
            return Categories.FirstOrDefault(c => c.Slug.Equals(slug, StringComparison.CurrentCulture));
        }

        public PhotoModel FindPhotoBySlug(string slug) {
            return Photos.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.CurrentCulture));
        }

        public List<PhotoModel> FindPhotosByCategorySlug(string slug, int page, int size) {
            var query = Photos.Where(p => p.IsApproved && p.Category.Slug.Equals(slug, StringComparison.CurrentCulture)).OrderByDescending(p => p.CreationDate);
            return query.Skip((page - 1)*size).Take(size).ToList();
        }

        public List<PhotoModel> FindPhotosByUserSlug(string slug, int page, int size) {
            var query = Photos.Where(p => p.IsApproved && p.User.Slug.Equals(slug, StringComparison.CurrentCulture)).OrderByDescending(p => p.CreationDate);
            return query.Skip((page - 1)*size).Take(size).ToList();
        }

        public List<PhotoModel> FindPhotosByQuery(string queryText, int page, int size) {
            var query = Photos.Where(p => p.IsApproved && (p.Title.Contains(queryText) || p.Slug.Contains(queryText) || p.Description.Contains(queryText) || p.Category.Name.Contains(queryText) || p.Category.Slug.Contains(queryText) || p.User.DisplayName.Contains(queryText) || p.User.Slug.Contains(queryText))).OrderByDescending(p => p.CreationDate);
            return query.Skip((page - 1)*size).Take(size).ToList();
        }
        
        public List<PhotoModel> FindPhotos(int page, int size) {
            var query = Photos.Where(p => p.IsApproved).OrderByDescending(p => p.CreationDate);
            return query.Skip((page - 1)*size).Take(size).ToList();
        }

        public int FindPhotoCountsByCategoryId(int categoryId) {
            return Photos.Count(p => p.IsApproved && p.CategoryId == categoryId);
        }

        public int FindPhotoCountsByUserId(int userId) {
            return Photos.Count(p => p.IsApproved && p.UserId == userId);
        }

        public void CreateUser(string email, string slug, string password, string displayName) {
            var user = FindUserByEmail(email);

            if (user != null)
                throw new DbValidationException("لطفا یک ایمیل دیگر وارد کنید.");

            user = FindUserBySlug(slug);

            if (user != null)
                throw new DbValidationException("لطفا یک نامک دیگر وارد کنید.");

            user = new UserModel {Email = email, Slug = slug, Password = password, DisplayName = displayName};

            Add(user);
            SaveChanges();
        }

        public bool ValidateUser(string email, string password) {
            var user = FindUserByEmail(email);

            return user != null && string.Equals(user.Password, password, StringComparison.CurrentCulture);
        }

        public void ChangeUserPassword(UserModel user, string currentPassword, string newPassword) {
            if (!ValidateUser(user.Email, currentPassword))
                throw new DbValidationException("گذرواژه فعلی رو درست وارد نکردی.");

            user.Password = newPassword;
            SaveChanges();
        }

        public IDictionary<CategoryModel, int> FindCategoriesCloud() {
            return Categories.GroupJoin(Photos, c => c.CategoryId, p => p.CategoryId, (c, p) => new {Key = c, Value = p.Count(p2 => p2.IsApproved)}).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public int FindPhotosCount() {
            return Photos.Count(p => p.IsApproved);
        }

        public List<CommentModel> FindCommentsByPhotoId(int photoId) {
            return Comments.Where(c => c.PhotoId == photoId).ToList();
        }

        public List<CommentModel> FindCommentsByUserId(int userId) {
            return Comments.Where(c => c.UserId == userId).ToList();
        }

        public List<CommentModel> FindCommentsByPhotoId(int photoId, int page, int size) {
            var query = Comments.Where(c => c.PhotoId == photoId && c.IsApproved).OrderByDescending(c => c.PostDate);
            return query.Skip((page - 1)*size).Take(size).ToList();
        }

        public int FindCommentsCountByPhotoId(int photoId) {
            return Comments.Count(c => c.PhotoId == photoId && c.IsApproved);
        }

        public List<UserPhotoActionModel> FindUserPhotoActionsByPhotoId(int photoId) {
            return UserPhotoActions.Where(upa => upa.PhotoId == photoId).ToList();
        }

        public List<UserPhotoActionModel> FindUserPhotoActionsByUserId(int userId) {
            return UserPhotoActions.Where(upa => upa.UserId == userId).ToList();
        }

        public List<UserModel> FindUserPhotoActionsByType(int photoId, UserPhotoActionType actionType) {
            return UserPhotoActions.Where(upa => upa.PhotoId == photoId && upa.ActionType == actionType).Select(upa => upa.User).ToList();
        }

        public int FindUserPhotoActionsCountByType(int photoId, UserPhotoActionType actionType) {
            return UserPhotoActions.Count(upa => upa.PhotoId == photoId && upa.ActionType == actionType);
        }

        public bool HasUserPhotoAction(int userId, int photoId, UserPhotoActionType actionType) {
            return UserPhotoActions.Any(upa => upa.UserId == userId && upa.PhotoId == photoId && upa.ActionType == actionType);
        }

        public void CreateUserPhotoAction(int? userId, int photoId, UserPhotoActionType actionType, bool preventDuplicate) {
            if (preventDuplicate && userId != null && HasUserPhotoAction((int)userId, photoId, actionType))
                return;

            var userPhotoAction = new UserPhotoActionModel {UserId = userId, PhotoId = photoId, ActionType = actionType};
            UserPhotoActions.Add(userPhotoAction);
            SaveChanges();
        }

        public IEnumerable<SelectListItem> GetCategoriesList(int? selectedCategoryId = null) {
            return Categories.Select(c => new SelectListItem {Text = c.Name, Value = c.CategoryId.ToString(), Selected = c.CategoryId == selectedCategoryId}).ToList();
        }
    }
}