using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TRepair
    {
        public int CRepairNumber { get; set; }
        public int CEmployeeId { get; set; }
        public DateTime CAppleDate { get; set; }
        public string CRepairCategory { get; set; }
        public string CLocation { get; set; }
        public string CContentofRepair { get; set; }
        public string CPhone { get; set; }
        public byte CRepairStatus { get; set; }

        public virtual TUser CEmployee { get; set; }
    }
}
