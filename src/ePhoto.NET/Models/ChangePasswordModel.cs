using System.ComponentModel.DataAnnotations;

namespace ePhoto.NET.Models {
    public class ChangePasswordModel {
        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه فعلی")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداقل {2} حرف باشد.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "گذرواژه جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار گذرواژه جدید")]
        [Compare("NewPassword", ErrorMessage = "گذرواژه جدید و تکرارش باید یکسان باشند.")]
        public string ConfirmNewPassword { get; set; }
    }
}