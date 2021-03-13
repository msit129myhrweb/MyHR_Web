using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TUserOnBoardStatus
    {
        public TUserOnBoardStatus()
        {
            TUsers = new HashSet<TUser>();
        }

        public int COnBoardStatusId { get; set; }
        public string COnBoardStatus { get; set; }

        public virtual ICollection<TUser> TUsers { get; set; }
    }
}
