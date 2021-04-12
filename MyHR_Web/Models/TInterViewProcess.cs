using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TInterViewProcess
    {
        public int CInterViewProcessKey { get; set; }
        public int CInterViewProcessId { get; set; }
        public string CInterViewProcess { get; set; }
        public string CProcessTime { get; set; }

        public virtual TInterView CInterViewProcessNavigation { get; set; }
    }
}
