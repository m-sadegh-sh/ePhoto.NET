using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePhoto.NET.Models {
    [Table("Users")]
    public class UserModel {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64)]
        public string Email { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64)]
        public string Slug { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64)]
        public string Password { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64)]
        public string DisplayName { get; set; }

        [DisplayName("تصویر")]
        public byte[] AvatarFile { get; set; }

        public string AvatarMimeType { get; set; }

        public virtual List<UserPhotoActionModel> Actions { get; set; }

        public bool IsAdmin { get; set; }
    }
}