using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ePhoto.NET.Models {
    public class NewPhotoModel {
        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DisplayName("عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DisplayName("نامک")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "فایلی پیوست نشده است.")]
        [DisplayName("تصویر")]
        public HttpPostedFileBase PhotoFile { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("توضیحی مختصر")]
        public string Description { get; set; }

        [DisplayName("دسته بندی")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}