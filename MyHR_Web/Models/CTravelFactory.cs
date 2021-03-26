using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Models
{
    public class CTravelFactory
    {
        public List<TTravelExpenseApplication> getAll()
        {
            return getBysql("SELECT * FROM tTravel_Expense_Application", null);
        }

        private List<TTravelExpenseApplication> getBysql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source =.; Initial Catalog = dbMyCompany; Integrated Security = True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                {
                    cmd.Parameters.Add(p);
                }
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<TTravelExpenseApplication> list = new List<TTravelExpenseApplication>();
            while (reader.Read())
            {
                TTravelExpenseApplication travel = new TTravelExpenseApplication()
                {
                    //todo Reina
                    //CDepartment = (int)reader["CDepartment"],
                    CEmployeeId = (int)reader["CEmployeeId"],
                    CReason = reader["CReason"].ToString(),
                    CApplyDate = (string)reader["CApplyDate"],
                    CTravelStartTime = (string)reader["CTravelStartTime"],
                    CTravelEndTime = (string)reader["CTravelEndTime"],
                    CAmont = (decimal)reader["CAmont"],
                    CCheckStatus = (int)reader["CCheckStatus"]
                };
                list.Add(travel);
            }
            con.Close();
            return list;
        }

        public TTravelExpenseApplication getById(int cApplyNumber)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("K_CApplyNumber", (object)cApplyNumber));
            string sql = "SELECT * FROM tTravel_Expense_Application WHERE cApplyNumber=" + cApplyNumber.ToString();
            List<TTravelExpenseApplication> listresult = getBysql(sql, list);
            if (listresult.Count == 0)
            {
                return null;
            }
            return listresult[0];
        }
        public void instert(TTravelExpenseApplication t)
        {
            string sql = "INSERT INTO tTravel_Expense_Application(";
            sql += "cDepartment,";
            sql += "cEmployeeId,";
            sql += "cReason,";
            sql += "cApplyDate,";
            sql += "cTravelStartTime,";
            sql += "cTravelEndTime,";
            sql += "cAmont,";
            sql += "cCheckStatus";
            sql += ")VALUES(";
            sql += "@K_CDepartment,";
            sql += "@K_CEmployeeId,";
            sql += "@K_CReason,";
            sql += "@K_CApplyDate,";
            sql += "@K_CTravelStartTime,";
            sql += "@K_CTravelEndTime,";
            sql += "@K_CAmont,";
            sql += "@K_CCheckStatus";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("K_CDepartment", (object)t.CDepartment));
            list.Add(new SqlParameter("K_CEmployeeId", (object)t.CEmployeeId));
            list.Add(new SqlParameter("K_CReason", (object)t.CReason));
            list.Add(new SqlParameter("K_CApplyDate", (object)t.CApplyDate));
            list.Add(new SqlParameter("K_CTravelStartTime", (object)t.CTravelStartTime));
            list.Add(new SqlParameter("K_CTravelEndTime", (object)t.CTravelEndTime));
            list.Add(new SqlParameter("K_CAmont", (object)t.CAmont));
            list.Add(new SqlParameter("K_CCheckStatus", (object)t.CCheckStatus));
            list.Add(new SqlParameter("K_CApplyNumber", (object)t.CApplyNumber));

            execusteSql(sql, list);
        }

        private void execusteSql(string sql, List<SqlParameter> paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbMyCompany;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
            {
                foreach (SqlParameter p in paras)
                {
                    cmd.Parameters.Add(p);
                }
            }
            cmd.ExecuteNonQuery();
            con.Close();
        }
        internal List<TTravelExpenseApplication> getByKeyword(string keyword)
        {

            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter("K_CReason", "%" + (object)keyword + "%"));
            list.Add(new SqlParameter("K_CApplyDate", "%" + (object)keyword + "%"));
            list.Add(new SqlParameter("K_CTravelStartTime", "%" + (object)keyword + "%"));
            list.Add(new SqlParameter("K_CTravelEndTime", "%" + (object)keyword + "%"));
            list.Add(new SqlParameter("K_CAmont", "%" + (object)keyword + "%"));
            list.Add(new SqlParameter("K_CCheckStatus", "%" + (object)keyword + "%"));
            string sql = "SELECT * FROM tTravel_Expense_Application WHERE cReason LIKE @K_CReason";
            sql += " OR cApplyDate LIKE @K_CApplyDate";
            sql += " OR cTravelStartTime LIKE @K_CTravelStartTime";
            sql += " OR cTravelEndTime LIKE @K_CTravelEndTime";
            sql += " OR cAmont LIKE @K_CAmont";
            sql += " OR cCheckStatus LIKE @K_CCheckStatus";
            return getBysql(sql, list);

        }

        public void delete(TTravelExpenseApplication t)
        {
            string sql = "DELETE FROM tTravel_Expense_Application WHERE cApplyNumber=@K_CApplyNumber";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("K_CApplyNumber", (object)t.CApplyNumber));
            execusteSql(sql, list);
        }

        public void update(TTravelExpenseApplication t)
        {
            string sql = "Update tTravel_Expense_Application SET ";
            sql += "cDepartment=@K_CDepartment,";
            sql += "cEmployeeId=@K_CEmployeeId,";
            sql += "cReason=@K_CReason,";
            sql += "cApplyDate=@K_CApplyDate,";
            sql += "cTravelStartTime=@K_CTravelStartTime,";
            sql += "cTravelEndTime=@K_CTravelEndTime,";
            sql += "cAmont=@K_CAmont,";
            sql += "cCheckStatus=@K_CCheckStatus";
            sql += " WHERE cApplyNumber=@K_CApplyNumber";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("K_CDepartment", (object)t.CDepartment));
            list.Add(new SqlParameter("K_CEmployeeId", (object)t.CEmployeeId));
            list.Add(new SqlParameter("K_CReason", (object)t.CReason));
            list.Add(new SqlParameter("K_CApplyDate", (object)t.CApplyDate));
            list.Add(new SqlParameter("K_CTravelStartTime", (object)t.CTravelStartTime));
            list.Add(new SqlParameter("K_CTravelEndTime", (object)t.CTravelEndTime));
            list.Add(new SqlParameter("K_CAmont", (object)t.CAmont));
            list.Add(new SqlParameter("K_CCheckStatus", (object)t.CCheckStatus));
            list.Add(new SqlParameter("K_CApplyNumber", (object)t.CApplyNumber));

            execusteSql(sql, list);
        }
    }    
}
