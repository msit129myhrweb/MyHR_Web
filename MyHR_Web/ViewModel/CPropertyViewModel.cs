using Microsoft.AspNetCore.Http;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CPropertyViewModel
    {
        private TLostAndFound iv_property = null;
        private TLostAndFoundSubject iv_subject = null;
        private TLostAndFoundCategory iv_category = null;
        private TLostAndFoundCheckStatus iv_status = null;
        public TLostAndFound property { get { return iv_property; } }
        public CPropertyViewModel(TLostAndFound l, TLostAndFoundSubject s, TLostAndFoundCategory c, TLostAndFoundCheckStatus e)
        {
            iv_property = l;
            iv_subject = s;
            iv_category = c;
            iv_status = e;
        }

        public CPropertyViewModel()
        {
            iv_property = new TLostAndFound();
            iv_subject = new TLostAndFoundSubject();
            iv_category = new TLostAndFoundCategory();
            iv_status = new TLostAndFoundCheckStatus();
        }
        [DisplayName("失物編號")]
        public int CPropertyId
        {
            get { return iv_property.CPropertyId; }
            set { iv_property.CPropertyId = value; }
        }

       
        [Required(ErrorMessage = "部門是必填欄位")]
        [DisplayName("部門")]
        public string CDepartmentName { get; set; }
        public int CDeparmentId
        {
            get { return iv_property.CDeparmentId;}
            set { iv_property.CDeparmentId = value; }
        }
        [Required(ErrorMessage = "員編是必填欄位")]
        [DisplayName("員編")]
        public int CEmployeeId
        {
            get { return iv_property.CEmployeeId;}
            set { iv_property.CEmployeeId = value; }
        }
        [Required(ErrorMessage = "手機是必填欄位")]
        [DisplayName("手機")]
        public string CPhone
        {
            get {return iv_property.CPhone; }
            set { iv_property.CPhone = value; }
        }
        [Required(ErrorMessage = "失物主旨是必填欄位")]
        [DisplayName("失物主旨")]
        public int CPropertySubjectId
        {
            get { return iv_property.CPropertySubjectId; }
            set { iv_property.CPropertySubjectId = value; }
        }
        [Required(ErrorMessage = "類別是必填欄位")]
        [DisplayName("類別")]
        public int CPropertyCategoryId
        {
            get { return iv_property.CPropertyCategoryId; }
            set { iv_property.CPropertyCategoryId = value; }
        }
        [Required(ErrorMessage = "失物照片是必填欄位")]
        [DisplayName("失物照片")]
        public string CPropertyPhoto
        {
            get { return iv_property.CPropertyPhoto; }
            set { iv_property.CPropertyPhoto = value; }
        }
        public IFormFile image { get; set; }

        [Required(ErrorMessage = "失物名稱是必填欄位")]
        [DisplayName("失物名稱")]
        public string CProperty
        {
            get { return iv_property.CProperty; }
            set { iv_property.CProperty = value; }
        }
        [Required(ErrorMessage = "日期是必填欄位")]
        [DisplayName("日期")]
        public DateTime? CLostAndFoundDate
        {
            get { return iv_property.CLostAndFoundDate; }
            set { iv_property.CLostAndFoundDate = value; }
        }
        [Required(ErrorMessage = "地點是必填欄位")]
        [DisplayName("地點")]
        public string CLostAndFoundSpace
        {
            get { return iv_property.CLostAndFoundSpace; }
            set { iv_property.CLostAndFoundSpace = value; }
        }
        [Required(ErrorMessage = "失物描述是必填欄位")]
        [DisplayName("失物描述")]
        public string CtPropertyDescription
        {
            get { return iv_property.CtPropertyDescription; }
            set { iv_property.CtPropertyDescription = value; }
        }

        [DisplayName("失物狀態")]
        public string CPropertyCheckStatusName {get ;set ; }

        public eLostAndFoundCheckStatus CPropertyCheckStatusId
        {
            get { return (eLostAndFoundCheckStatus)iv_property.CPropertyCheckStatusId; }
            set { iv_property.CPropertyCheckStatusId = (int)value; }
        }

    }
}
