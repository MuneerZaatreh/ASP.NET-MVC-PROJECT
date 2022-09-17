using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expose_Tracker.Models
{

    public class Transaction
    {
        [Key]

        public int TransactionId { get; set; } = 0;
        //categoryID
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0")]
        public int Amount { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string? CategiryTitleWithIcon
        {
            get
            {
                return Category == null ? "" : Category.Icon + " " + Category.Title;
            }
        }
        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null || Category.Type == "Expense") ? "-" : "+") + Amount.ToString("C0");
            }
        }

    }
}
