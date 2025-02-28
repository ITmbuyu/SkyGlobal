using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SkyGlobal.Models;

namespace SkyGlobal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<PayrollAdjustment> PayrollAdjustments { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        public DbSet<KpiEntry> KpiEntries { get; set; }
        public DbSet<TeamKpiPerformance> TeamKpiPerformances { get; set; }
        public DbSet<OvertimeEntry> OvertimeEntries { get; set; }
        public DbSet<AttendanceBonus> AttendanceBonuses { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<PayslipQuery> PayslipQueries { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
		public DbSet<LeaveType> LeaveTypes { get; set; }
		public DbSet<LeaveStatus> LeaveStatuses { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<WorkUpdateTopic> WorkUpdateTopics { get; set; }

        public DbSet<WorkUpdate> WorkUpdates { get; set; }

        public DbSet<EventCategorie> EventCategories { get; set; }

        public DbSet<Event> Events { get; set; }

    }
}
