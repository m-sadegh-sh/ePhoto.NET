using System.ComponentModel.DataAnnotations;

namespace ePhoto.NET.Models {
    public class LoginModel {
        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه")]
        public string Password { get; set; }

        [Display(Name = "منو یادت باشه.")]
        public bool RememberMe { get; set; }
    }
}