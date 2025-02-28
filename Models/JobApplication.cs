using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyGlobal.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int JobPostingId { get; set; }

        [ForeignKey(nameof(JobPostingId))]
        public virtual JobPosting Job { get; set; }

        [Required, StringLength(50)]
        public string ApplicantName { get; set; }

        [Required, EmailAddress]
        public string ApplicantEmail { get; set; }

        [DataType(DataType.Upload)]
        public string ResumePath { get; set; } // Store uploaded resume path

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        [DataType(DataType.DateTime)]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [DataType(DataType.MultilineText)]
        public string CoverLetter { get; set; }
    }

}
