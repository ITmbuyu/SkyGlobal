namespace SkyGlobal.Models
{
    public class Team
    {
        public int TeamId { get; set; }  // PK
        public string TeamLeaderId { get; set; }  // FK to Users
        public string TeamName { get; set; }
    }
}