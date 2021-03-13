using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TLostAndFoundCategory
    {
        public TLostAndFoundCategory()
        {
            TLostAndFounds = new HashSet<TLostAndFound>();
        }

        public int CPropertyCategoryId { get; set; }
        public string CPropertyCategory { get; set; }

        public virtual ICollection<TLostAndFound> TLostAndFounds { get; set; }
    }
}
