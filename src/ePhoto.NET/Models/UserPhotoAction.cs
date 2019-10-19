using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePhoto.NET.Models {
    [Table("UserPhotoActions")]
    public class UserPhotoActionModel {
        [Key]
        public int UserPhotoId { get; set; }

        public int? UserId { get; set; }
        public virtual UserModel User { get; set; }

        public int PhotoId { get; set; }
        public virtual PhotoModel Photo { get; set; }

        public UserPhotoActionType ActionType { get; set; }
    }
}