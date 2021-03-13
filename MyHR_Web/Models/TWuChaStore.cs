using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TWuChaStore
    {
        public TWuChaStore()
        {
            TWuChaItems = new HashSet<TWuChaItem>();
            TWuChaOrders = new HashSet<TWuChaOrder>();
        }

        public int CStoreId { get; set; }
        public string CStoreName { get; set; }

        public virtual ICollection<TWuChaItem> TWuChaItems { get; set; }
        public virtual ICollection<TWuChaOrder> TWuChaOrders { get; set; }
    }
}
