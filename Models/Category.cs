using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expose_Tracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        [Required(ErrorMessage ="Title is Requird")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";
        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expense";
        [NotMapped]
        public String? TitleWithIcon { 
            
           get
            {
                return this.Icon + " " + this.Title;
            }
        }




    }
}
