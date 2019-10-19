using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePhoto.NET.Models {
    [Table("Photos")]
    public class PhotoModel {
        [Key]
        public int PhotoId { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DisplayName("عنوان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [DisplayName("نامک")]
        public string Slug { get; set; }

        [DisplayName("تصویر")]
        public byte[] PhotoFile { get; set; }

        public string PhotoMimeType { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("توضیحی مختصر")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public int UserId { get; set; }
        public virtual UserModel User { get; set; }

        public int CategoryId { get; set; }
        public virtual CategoryModel Category { get; set; }
        
        public virtual List<UserPhotoActionModel> Actions { get; set; }

        public bool IsApproved { get; set; }
    }
}