using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class BuilderLeave
    { 
        public class Home
        {
            public string Address { get; set; }
            public List<TLeaveApplicationViewModel> LeaveApplication { get; set; }
            public List<TLeave> Leave { get; set; }
           
        }



    }
}
