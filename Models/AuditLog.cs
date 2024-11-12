namespace SkyGlobal.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public string Action { get; set; }
        public DateTime Date { get; set; }
        public string Details { get; set; }
    }
}