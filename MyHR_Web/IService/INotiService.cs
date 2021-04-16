using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.IService
{
    public interface INotiService
    {
        List<Noti> GetNotifications(int nToUserId, bool bIsGetOnlyUnread);
    }
}
