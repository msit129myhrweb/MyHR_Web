using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CInterviewListViewModel
    {
        dbMyCompanyContext myHR = new dbMyCompanyContext();
        private TInterView iv_interview = null;
        public TInterView interview { get { return iv_interview; } }
        public CInterviewListViewModel(TInterView i)
        {
            iv_interview = i;
        }
        public CInterviewListViewModel()
        {             
            iv_interview = new TInterView();
        }
        [DisplayName("編號")]
        public int CInterVieweeId { get { return iv_interview.CInterVieweeId; } set { iv_interview.CInterVieweeId = value; } }
        [DisplayName("性別")]
        public string CInterVieweeGender { get { return iv_interview.CInterVieweeGender; } set { iv_interview.CInterVieweeGender = value; } }
        [DisplayName("姓名")]
        public string CInterVieweeName { get { return iv_interview.CInterVieweeName; } set { iv_interview.CInterVieweeName = value; } }
        [DisplayName("英文名")]
        public string CEmployeeEnglishName { get { return iv_interview.CEmployeeEnglishName; } set { iv_interview.CEmployeeEnglishName = value; } }
        [DisplayName("電話")]
        public string CPhone { get { return iv_interview.CPhone; } set { iv_interview.CPhone = value; } }
        [DisplayName("電子郵件")]
        public string CEmail { get { return iv_interview.CEmail; } set { iv_interview.CEmail = value; } }
        [DisplayName("住址")]
        public string CAddress { get { return iv_interview.CAddress; } set { iv_interview.CAddress = value; } }
        [DisplayName("生日")]
        public string CBirthday { get { return iv_interview.CBirthday; } set { iv_interview.CBirthday = value; } }
        [DisplayName("年齡")]
        public int CAge { get { return iv_interview.CAge; } set { iv_interview.CAge = value; } }
        public byte[]? CPhoto { get { return iv_interview.CPhoto; } set { iv_interview.CPhoto = value; } }
        [DisplayName("學歷")]
        public string CEducation { get { return iv_interview.CEducation; } set { iv_interview.CEducation = value; } }
        public string CExperience { get { return iv_interview.CExperience; } set { iv_interview.CExperience = value; } }        
        public int CJobTitle { get { return iv_interview.CJobTitle; } set { iv_interview.CJobTitle = value; } }
        [DisplayName("應徵職稱")]
        public string CJobTitleName
        {
            get
            {
                string _CJobTitleName = myHR.TUserJobTitles.Where(n => n.CJobTitleId == CJobTitle).Select(n => n.CJobTitle).FirstOrDefault();
                return _CJobTitleName;
            }
            set { CJobTitleName = value; }
        }
        public int CDepartment { get { return iv_interview.CDepartment; } set { iv_interview.CDepartment = value; } }
        [DisplayName("應徵部門")]
        public string CDepartmentName
        {
            get
            {
                string _CDepartmentName = myHR.TUserDepartments.Where(n => n.CDepartmentId == CDepartment).Select(n => n.CDepartment).FirstOrDefault();
                return _CDepartmentName;
            }
            set { CDepartmentName = value; }
        }
        public int? CInterViewerEmployeeId { get { return iv_interview.CInterViewerEmployeeId; } set { iv_interview.CInterViewerEmployeeId = value; } }
        [DisplayName("建檔日")]
        public string CInterViewDate { get { return iv_interview.CInterViewDate; } set { iv_interview.CInterViewDate = value; } }        
        public int CInterViewStatusId { get { return iv_interview.CInterViewStatusId; } set { iv_interview.CInterViewStatusId = value; } }
        [DisplayName("狀態")]
        public string CStatus
        {
            get
            {
                string _CStatus = myHR.TInterViewStatuses.Where(n => n.InterViewStatusId == CInterViewStatusId).Select(n => n.InterViewStatus).FirstOrDefault();
                return _CStatus;
            }
            set { CStatus = value; }
        }
        public int? CInterViewProcessId { get { return iv_interview.CInterViewProcessId; } set { iv_interview.CInterViewProcessId = value; } }
    }
}
