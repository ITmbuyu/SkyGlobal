
using System.ComponentModel.DataAnnotations;
using System;
namespace SkyGlobal.Models
{
    public class JobPosting
    {
        [Key]
        public int JobPostingId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Requirements { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        public bool IsInternal { get; set; }  // True = Internal, False = External

        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }
    }

}
