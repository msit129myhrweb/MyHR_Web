using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TWuChaOrder
    {
        public int CWuChaOrderNumber { get; set; }
        public string CGroupId { get; set; }
        public int CStoreId { get; set; }
        public int CEmployeeId { get; set; }
        public DateTime CDate { get; set; }
        public int CTotalPirce { get; set; }

        public virtual TUser CEmployee { get; set; }
        public virtual TWuChaStore CStore { get; set; }
    }
}
