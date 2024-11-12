namespace SkyGlobal.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsRead { get; set; }
    }
}