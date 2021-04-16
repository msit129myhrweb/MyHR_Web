using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class ViewTNotification
    {
        public int CNotiId { get; set; }
        public int? CFromUserId { get; set; }
        public int? CToUserId { get; set; }
        public string CNotiHeader { get; set; }
        public string CNotiBody { get; set; }
        public bool? CIsRead { get; set; }
        public string CUrl { get; set; }
        public DateTime? CCreateDate { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
    }
}
