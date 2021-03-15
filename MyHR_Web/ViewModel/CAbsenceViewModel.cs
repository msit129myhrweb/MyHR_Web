using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CAbsenceViewModel
    {
        private TAbsence iv_absence = null;
        public TAbsence absence { get { return iv_absence; } }
        public CAbsenceViewModel(TAbsence a)
        {
            iv_absence = a;
        }
        public int CApplyNumber { get; set; }
        public int CEmployeeID { get; set; }
        [DisplayName("上班")]
        public DateTime COn { get; set; }
        [DisplayName("下班")]
        public DateTime COff { get; set; }
    }
}
