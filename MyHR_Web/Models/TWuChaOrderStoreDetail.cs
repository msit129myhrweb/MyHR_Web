using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TWuChaOrderStoreDetail
    {
        public int CWuChaOrderNumber { get; set; }
        public int CStoreId { get; set; }
        public int CItemId { get; set; }
        public int CItemQuantity { get; set; }

        public virtual TWuChaItem CItem { get; set; }
        public virtual TWuChaStore CStore { get; set; }
        public virtual TWuChaOrder CWuChaOrderNumberNavigation { get; set; }
    }
}
