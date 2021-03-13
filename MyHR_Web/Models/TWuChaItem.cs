using System;
using System.Collections.Generic;

#nullable disable

namespace MyHR_Web.Models
{
    public partial class TWuChaItem
    {
        public int CItemId { get; set; }
        public int CStoreId { get; set; }
        public string CItemName { get; set; }
        public int CItemPrice { get; set; }

        public virtual TWuChaStore CStore { get; set; }
    }
}
