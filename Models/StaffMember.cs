namespace SkyGlobal.Models
{
    public class StaffMember
    {
       public int StaffMemberId { get; set; }
       public string StaffMemberName { get; set; }
       public string StaffMemberSurname { get; set; }
       public string StaffMemberEmail { get; set; }

       public string StaffMemberPhone { get; set; }

       public string StaffMemberPassword { get; set; }

       public string StaffMemberRole { get; set; }

       public double StaffMemeberRate { get; set; }

       public int StaffMemeberHours { get; set; }

        public int? TeamId { get; set; }
        public virtual Team? Team { get; set; }



    }
}