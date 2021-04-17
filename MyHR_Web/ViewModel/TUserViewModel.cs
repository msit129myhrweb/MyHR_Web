using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class TUserViewModel
    {
        private TUser iv_user = null;
        public TUser tuserVM { get { return iv_user; } }
        public TUserViewModel(TUser p)
        {
            iv_user = p;
        }
        public TUserViewModel()
        {
            iv_user = new TUser();
        }
        [DisplayName("員工編號")]
        public int CEmployeeId
        {
            get { return iv_user.CEmployeeId; }
            set { iv_user.CEmployeeId = value; }
        }

        [DisplayName("姓名")]
        public string CEmployeeName
        {
            get { return iv_user.CEmployeeName; }
            set { iv_user.CEmployeeName = value; }
        }
        [DisplayName("英文姓名")]
        public string CEmployeeEnglishName
        {
            get { return iv_user.CEmployeeEnglishName; }
            set { iv_user.CEmployeeEnglishName = value; }
        }
        [DisplayName("密碼")]
        public string CPassWord
        {
            get { return iv_user.CPassWord; }
            set { iv_user.CPassWord = value; }
        }
        [DisplayName("到職日")]
        public DateTime COnBoardDay
        {
            get { return iv_user.COnBoardDay; }
            set { iv_user.COnBoardDay = value; }
        }
        [DisplayName("離職日")]
        public DateTime? CByeByeDay
        {
            get { return iv_user.CByeByeDay; }
            set { iv_user.CByeByeDay = value; }
        }
        [DisplayName("性別")]
        public string CGender
        {
            get { return iv_user.CGender; }
            set { iv_user.CGender = value; }
        }
        [DisplayName("Email")]
        public string CEmail
        {
            get { return iv_user.CEmail; }
            set { iv_user.CEmail = value; }
        }
        [DisplayName("地址")]
        public string CAddress
        {
            get { return iv_user.CAddress; }
            set { iv_user.CAddress = value; }
        }
        [DisplayName("部門")]
        public int CDepartmentId
        {
            get { return iv_user.CDepartmentId; }
            set { iv_user.CDepartmentId = value; }
        }
        [DisplayName("職務")]
        public int CJobTitleId
        {
            get { return iv_user.CJobTitleId; }
            set { iv_user.CJobTitleId = value; }
        }
        [DisplayName("主管")]
        public int? CSupervisor
        {
            get { return iv_user.CSupervisor; }
            set { iv_user.CSupervisor = value; }
        }
        [DisplayName("生日")]
        public DateTime? CBirthday
        {
            get { return iv_user.CBirthday; }
            set { iv_user.CBirthday = value; }
        }
        [DisplayName("電話")]
        public string CPhone
        {
            get { return iv_user.CPhone; }
            set { iv_user.CPhone = value; }
        }
        [DisplayName("照片")]
        public byte[] CPhoto
        {
            get { return iv_user.CPhoto; }
            set { iv_user.CPhoto = value; }
        }
        [DisplayName("緊急聯絡人")]
        public string CEmergencyPerson
        {
            get { return iv_user.CEmergencyPerson; }
            set { iv_user.CEmergencyPerson = value; }
        }
        [DisplayName("緊急連絡電話")]
        public string CEmergencyContact
        {
            get { return iv_user.CEmergencyContact; }
            set { iv_user.CEmergencyContact = value; }
        }
        [DisplayName("到職狀態")]
        public int COnBoardStatusId
        {
            get { return iv_user.COnBoardStatusId ; }
            set { iv_user.COnBoardStatusId = value; }
        }
        [DisplayName("帳號狀態")]
        public byte? CAccountEnable
        {
            get { return iv_user.CAccountEnable; }
            set { iv_user.CAccountEnable = value; }
        }
    }
}
