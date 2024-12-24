﻿namespace SkyGlobal.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }  // PK
        public string? UserId { get; set; }  // FK to Users 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }  // Pending, Approved, Declined
        public string? ApproverId { get; set; }  // Nullable FK to Users
        public string? ApproverRole { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? Comments { get; set; }
    }
}