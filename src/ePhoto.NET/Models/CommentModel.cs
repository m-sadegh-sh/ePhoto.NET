using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePhoto.NET.Models {
    [Table("Comments")]
    public class CommentModel {
        [Key]
        public int CommentId { get; set; }

        public int PhotoId { get; set; }
        public int UserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        public virtual PhotoModel Photo { get; set; }
        public virtual UserModel User { get; set; }

        public DateTime PostDate { get; set; }

        public bool IsApproved { get; set; }
    }
}