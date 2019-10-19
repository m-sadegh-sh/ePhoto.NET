using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

using ePhoto.NET.Helpers;

namespace ePhoto.NET.Models {
    public class MyProfileModel {
        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداکثر {1} حرف باشد.")]
        [RegularExpression(Patterns.Slug, ErrorMessage = "{0} را با قالب صحیح وارد نمائید.")]
        [Display(Name = "نامک")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64, ErrorMessage = "{0} باید حداکثر {1} حرف باشد.")]
        [Display(Name = "نام")]
        public string DisplayName { get; set; }

        [DisplayName("تصویر")]
        public HttpPostedFileBase AvatarFile { get; set; }
    }
}