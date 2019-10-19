using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePhoto.NET.Models {
    [Table("Categories")]
    public class CategoryModel {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64)]
        public string Name { get; set; }

        [Required(ErrorMessage = "این فیلد الزامیه.")]
        [StringLength(64)]
        public string Slug { get; set; }
    }
}