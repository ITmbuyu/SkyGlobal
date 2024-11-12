namespace SkyGlobal.Models
{
    public class AttendanceRecord
    {
        public int AttendanceRecordId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; }  // Present, Absent, etc.
        public string? AdjustedByUserId { get; set; }  // Nullable FK to Users
        public DateTime? ACheckInTime { get; set; }
        public DateTime? ACheckOutTime { get; set; }
    }
}