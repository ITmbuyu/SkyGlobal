namespace SkyGlobal.Models
{
    public class TeamKpiPerformance
    {
        public int TeamKpiPerformanceId { get; set; }  // PK
        public string TeamLeaderId { get; set; }  // FK to Users
        public DateTime Month { get; set; }
        public decimal AverageAHT { get; set; }
        public decimal AverageQA { get; set; }
        public decimal AverageConversionRate { get; set; }
        public decimal TeamPerformanceScore { get; set; }
    }
}