using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CInterviewEditViewModel
    {
        private TInterView iv_interview = null;
        public TInterView interview { get { return iv_interview; } }
        public CInterviewEditViewModel(TInterView i)
        {
            iv_interview = i;
        }
        public CInterviewEditViewModel()
        {
            iv_interview = new TInterView();
        }
        public int CInterVieweeId { get { return iv_interview.CInterVieweeId; } set { iv_interview.CInterVieweeId = value; } }
        public string CInterVieweeGender { get; set; }
        public string CInterVieweeName { get { return iv_interview.CInterVieweeName; } set { iv_interview.CInterVieweeName = value; } }
        public string CEmployeeEnglishName { get; set; }
        public string CPhone { get; set; }
        public string CEmail { get; set; }
        public string CAddress { get; set; }
        public string CBirthday { get; set; }
        public int CAge { get; set; }
        public byte[] CPhoto { get; set; }
        public string CEducation { get; set; }
        public string CExperience { get; set; }
        public int CJobTitle { get; set; }
        public int CDepartment { get; set; }
        public int CInterViewerEmployeeId { get; set; }
        public string CInterViewDate { get; set; }
        public int CInterViewStatusId { get; set; }
        public int? CInterViewProcessId { get; set; }
    }
}
