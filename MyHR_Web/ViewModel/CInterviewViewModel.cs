using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CInterviewViewModel
    {
        private TInterView iv_interview = null;
        public TInterView interview { get { return iv_interview; } }

        public CInterviewViewModel(TInterView i)
        {
            iv_interview = i;
        }
        public CInterviewViewModel()
        {
            iv_interview = new TInterView();
        }
        [DisplayName("品名")]
        public string CInterVieweeGender
        {
            get { return iv_interview.CInterVieweeGender; }
            set { iv_interview.CInterVieweeGender = value; }
        }
        
    }
}
