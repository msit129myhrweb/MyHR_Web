using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TEvent
    {
        public int EventId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public string ThemeColor { get; set; }
        public bool? IsFullDay { get; set; }
    }
}
