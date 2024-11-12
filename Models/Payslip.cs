namespace SkyGlobal.Models
{
    public class Payslip
    {
        public int PayslipId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime GeneratedDate { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
    }
}