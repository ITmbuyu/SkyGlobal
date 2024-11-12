namespace SkyGlobal.Models
{
    public class AttendanceBonus
    {
        public int AttendanceBonusId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime Month { get; set; }
        public decimal Amount { get; set; }

    }
}