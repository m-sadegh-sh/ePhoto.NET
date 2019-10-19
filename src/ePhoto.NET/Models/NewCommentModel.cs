using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ePhoto.NET.Models {
    public class NewCommentModel {
        public PhotoModel Photo { get; set; }

        [Required(ErrorMessage = "نظرت چیه؟")]
        [DisplayName("نظر شما...")]
        public string Body { get; set; }
    }
}