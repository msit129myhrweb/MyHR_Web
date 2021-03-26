using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TUserDepartment
    {
        public TUserDepartment()
        {
            TBulletins = new HashSet<TBulletin>();
            TInterViews = new HashSet<TInterView>();
            TLeaveApplications = new HashSet<TLeaveApplication>();
            TLostAndFounds = new HashSet<TLostAndFound>();
            TTravelExpenseApplications = new HashSet<TTravelExpenseApplication>();
            TUsers = new HashSet<TUser>();
        }

        public int CDepartmentId { get; set; }
        public string CDepartment { get; set; }

        public virtual ICollection<TBulletin> TBulletins { get; set; }
        public virtual ICollection<TInterView> TInterViews { get; set; }
        public virtual ICollection<TLeaveApplication> TLeaveApplications { get; set; }
        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
        public virtual ICollection<TTravelExpenseApplication> TTravelExpenseApplications { get; set; }
        public virtual ICollection<TUser> TUsers { get; set; }
    }
}
