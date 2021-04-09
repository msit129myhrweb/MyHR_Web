using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CInterviewListViewModel2
    {        
        [DisplayName("編號")]
        public int CInterVieweeId { get; set; }
        [DisplayName("性別")]
        public string CInterVieweeGender { get; set; }
        [DisplayName("姓名")]
        public string CInterVieweeName { get; set; }
        [DisplayName("英文名")]
        public string CEmployeeEnglishName { get; set; }
        [DisplayName("電話")]
        public string CPhone { get; set; }
        [DisplayName("電子郵件")]
        public string CEmail { get; set; }
        [DisplayName("住址")]
        public string CAddress { get; set; }
        [DisplayName("生日")]
        public string CBirthday { get; set; }
        [DisplayName("年齡")]
        public int CAge { get; set; }
        public byte[]? CPhoto { get; set; }
        [DisplayName("最高學歷")]
        public string CEducation { get; set; }
        public string CExperience { get; set; }
        public int CJobTitle { get; set; }
        [DisplayName("應徵職稱")]
        public string CJobTitleName { get; set; }
        public int CDepartment { get; set; }
        [DisplayName("應徵部門")]
        public string CDepartmentName { get; set; }

        public int CInterViewerEmployeeId { get; set; }
        public string CInterViewDate { get; set; }
        public int CInterViewStatusId { get; set; }
        [DisplayName("狀態")]
        public string CStatus { get; set; }

        public int? CInterViewProcessId { get; set; }
    }
}
