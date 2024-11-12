namespace SkyGlobal.Models
{
    public class KpiEntry
    {
        public int KpiEntryId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime WeekStartDate { get; set; }
        public decimal AHT { get; set; }
        public decimal QA { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal TotalCommission { get; set; }
        public string Status { get; set; }  // Pending, Approved, Declined
        public string? ApprovedByUserId { get; set; }  // Nullable FK to Users
    }
}