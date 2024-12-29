namespace SkyGlobal.Models
{
	public class LeaveStatus
	{
		public int LeaveStatusId { get; set; }  // PK
		public string? Status { get; set; }  // Pending, Approved, Declined
	}
}
