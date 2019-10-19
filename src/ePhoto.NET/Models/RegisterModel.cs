using System.ComponentModel.DataAnnotations;

using ePhoto.NET.Helpers;

namespace ePhoto.NET.Models {
    public class RegisterModel {
        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداکثر {1} حرف باشد.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداکثر {1} حرف باشد.")]
        [RegularExpression(Patterns.Slug, ErrorMessage = "{0} را با قالب صحیح وارد نمائید.")]
        [Display(Name = "نامک")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداقل {2} حرف باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار گذرواژه")]
        [Compare("Password", ErrorMessage = "گذرواژه و تکرارش باید یکسان باشند.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداکثر {1} حرف باشد.")]
        [Display(Name = "نام")]
        public string DisplayName { get; set; }
    }
}