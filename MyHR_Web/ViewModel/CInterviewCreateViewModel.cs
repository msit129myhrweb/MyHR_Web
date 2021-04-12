using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CInterviewCreateViewModel
    {
        public int CInterViewProcessId { get; set; }
        [DisplayName("面試記錄")]
        public string CInterViewProcess { get; set; }
        public string CProcessTime { get; set; }
    }
}
