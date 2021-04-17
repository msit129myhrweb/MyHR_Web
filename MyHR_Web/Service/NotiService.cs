using Dapper;
using Microsoft.Data.SqlClient;
using MyHR_Web.Common;
using MyHR_Web.IService;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Service
{
    public class NotiService : INotiService
    {
        List<Noti> _oNotifications = new List<Noti>();
        public List<Noti> GetNotifications(int nToUserId, bool bIsGetOnlyUnread)
        {
            _oNotifications = new List<Noti>();
            using(IDbConnection con=new SqlConnection(Global.ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var oNotis = con.Query<Noti>("select * from View_Notification where ToUserId="+ nToUserId).ToList();

                if (oNotis!=null && oNotis.Count()>0)
                {
                    _oNotifications = oNotis;
                }
                return _oNotifications;

            }
        }
    }
}
