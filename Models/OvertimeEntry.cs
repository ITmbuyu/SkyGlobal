namespace SkyGlobal.Models
{
    public class OvertimeEntry
    {
        public int OvertimeEntryId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}