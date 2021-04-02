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
      
        [DisplayName("失物編號")]
        public int CPropertyId { get; set; }
      
        [Required(ErrorMessage = "部門是必填欄位")]
        [DisplayName("部門")]
        public int CDeparmentId { get; set; }

        [Required(ErrorMessage = "員編是必填欄位")]
        [DisplayName("員編")]
        public int CEmployeeId { get; set; }
        [DisplayName("員工姓名")]
        public string CEmployeeName { get; set; }

        [Required(ErrorMessage = "手機是必填欄位")]
        [DisplayName("手機")]
        public string CPhone { get; set; }

        [Required(ErrorMessage = "失物主旨是必填欄位")]
        [DisplayName("失物主旨")]
        public int CPropertySubjectId { get; set; }

        [Required(ErrorMessage = "類別是必填欄位")]
        [DisplayName("類別")]
        public int CPropertyCategoryId { get; set; }

        //[Required(ErrorMessage = "失物照片是必填欄位")]
        [DisplayName("失物照片")]
        public string CPropertyPhoto { get; set; }
        public IFormFile image { get; set; }

        [Required(ErrorMessage = "失物名稱是必填欄位")]
        [DisplayName("失物名稱")]
        public string CProperty { get; set; }

        [Required(ErrorMessage = "日期是必填欄位")]
        [DisplayName("日期")]
        public DateTime? CLostAndFoundDate { get; set; }
        [Required(ErrorMessage = "地點是必填欄位")]
        [DisplayName("地點")]
        public string CLostAndFoundSpace { get; set; }

        [Required(ErrorMessage = "失物描述是必填欄位")]
        [DisplayName("失物描述")]
        public string CtPropertyDescription { get; set; }

        [DisplayName("失物狀態")]
        public int CPropertyCheckStatusId { get; set; }


    }
}
