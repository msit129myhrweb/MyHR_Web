using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TBulletin
    {
        public int CNumber { get; set; }
        public string CTitle { get; set; }
        public int CDepartment { get; set; }
        public string CCategory { get; set; }
        public string CContentofBulletin { get; set; }
        public DateTime CStarttime { get; set; }
        public DateTime CEndtime { get; set; }

        public virtual TUserDepartment CDepartmentNavigation { get; set; }
    }
}
