using System.Collections.Generic;

namespace ePhoto.NET.Models {
    public class PhotoDetailViewModel {
        public PhotoModel Photo { get; set; }
        public int PhotosCount { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int DownloadsCount { get; set; }
        public bool IsLiked { get; set; }
        public List<UserModel> LikedUsers { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}