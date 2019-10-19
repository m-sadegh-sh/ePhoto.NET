namespace ePhoto.NET.Models {
    public class PhotoViewModel {
        public PhotoModel Photo { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}