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
        public CAbsenceViewModel()
        {
            iv_absence = new TAbsence();
        }
        public int CApplyNumber 
        {
            get { return iv_absence.CApplyNumber; }
            set { iv_absence.CApplyNumber = value; } 
        }
        [DisplayName("員工編號")]
        public int CEmployeeId 
        {
            get { return iv_absence.CEmployeeId; }
            set { iv_absence.CEmployeeId = value; }
        }

        [DisplayName("上班")]
        public DateTime? COn 
        {
            get { return iv_absence.COn; }
            set { iv_absence.COn = value; }
        }
        [DisplayName("下班")]
        public DateTime? COff 
        {
            get { return iv_absence.COff; }
            set { iv_absence.COff = value; } 
        }
    }
}
