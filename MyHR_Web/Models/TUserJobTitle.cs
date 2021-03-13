using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TUserJobTitle
    {
        public TUserJobTitle()
        {
            TInterViews = new HashSet<TInterView>();
        }

        public int CJobTitleId { get; set; }
        public string CJobTitle { get; set; }

        public virtual ICollection<TInterView> TInterViews { get; set; }
    }
}
