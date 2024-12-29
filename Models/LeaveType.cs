namespace SkyGlobal.Models
{
	public class LeaveType
	{
		public int LeaveTypeId { get; set; }  //PK
		public string? Type { get; set; } //  Sick,  Annual, Paternity, Maternity, Bereavement, Accumulated
	}
}
