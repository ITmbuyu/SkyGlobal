namespace SkyGlobal.Models
{
    public class PayrollRecord
    {
        public int PayrollRecordId { get; set; }  // PK
        public string UserId { get; set; }  // FK to Users
        public DateTime SalaryDate { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal AttendanceDeduction { get; set; }
        public decimal Commission { get; set; }
        public decimal Bonus { get; set; }
        public decimal FinalSalary { get; set; }
    }
}