using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLostAndFoundSubject
    {
        public TLostAndFoundSubject()
        {
            TLostAndFounds = new HashSet<TLostAndFound>();
        }

        public int CPropertySubjectId { get; set; }
        public string CPropertySubject { get; set; }

        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
    }
}
