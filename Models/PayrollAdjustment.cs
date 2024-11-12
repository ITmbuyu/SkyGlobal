namespace SkyGlobal.Models
{
    public class PayrollAdjustment
    {
        public int PayrollAdjustmentId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string? ApprovedByUserId { get; set; }  // Nullable FK to Users
    }
}