namespace SkyGlobal.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime Workday { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOffDay { get; set; }
    }
}