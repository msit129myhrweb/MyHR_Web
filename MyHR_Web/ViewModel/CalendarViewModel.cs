using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.ViewModel
{
    public class CalendarViewModel
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        private TEvent iv_events = null;
        

        public TEvent tevents { get { return iv_events; } }
        public CalendarViewModel(TEvent e)
        {
            iv_events = e;
        }
        public CalendarViewModel()
        {
            iv_events = new TEvent();
        }


        public int EventId
        {
            get { return iv_events.EventId; }
            set { iv_events.EventId = value; }
        }
        public int EmployeeId
        {
            get { return iv_events.EmployeeId; }
            set { iv_events.EmployeeId = value; }
        }
        public string Subject
        {
            get { return iv_events.Subject; }
            set { iv_events.Subject = value; }
        }
        public string Description
        {
            get { return iv_events.Description; }
            set { iv_events.Description = value; }
        }
        public DateTime? Start
        {
            get { return iv_events.Start; }
            set { iv_events.Start = value; }
        }
        public Nullable<System.DateTime> End
        {
            get { return iv_events.End; }
            set { iv_events.End = value; }
        }
        public string ThemeColor
        {
            get { return iv_events.ThemeColor; }
            set { iv_events.ThemeColor = value; }
        }
        public bool? IsFullDay
        {
            get { return iv_events.IsFullDay; }
            set { iv_events.IsFullDay = value; }
        }
    }
}

