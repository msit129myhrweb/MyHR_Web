using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TEvent
    {
        public int EventId { get; set; }
        public int EmployeeId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string ThemeColor { get; set; }
        public bool? IsFullDay { get; set; }

        public virtual TUser Employee { get; set; }
    }
}
