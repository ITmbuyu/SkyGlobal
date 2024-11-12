namespace SkyGlobal.Models
{
    public class PayslipQuery
    {
        public int PayslipQueryId { get; set; }  // PK
        public int PayslipId { get; set; }  // FK to Payslips
        public string UserId { get; set; }  // FK to Users
        public string QueryType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }  // Pending, Approved, Declined
        public string? ResponseNotes { get; set; }
        public string? ReviewedByUserId { get; set; }  // Nullable FK to Users
    }
}