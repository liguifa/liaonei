using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MyBBS
{
    /// <summary>
    /// detailed 的摘要说明
    /// </summary>
    public class detailed : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["action"];
            int islogin = 0;
            if (!(context.Session["username"] == null))
            {
                islogin = 1;
            }

            if (action == "detailed")
            {
                long id = Convert.ToInt64(context.Request["ID"]);
                DataTable dt = SqlHelper.ExecuteDataTable("select * from  Table_3 where id=@id", new SqlParameter("id", @id));
                DataTable dt_2 = SqlHelper.ExecuteDataTable("select top 5 * from Publish where Goods='" + id + "'and Reply='0' order by id desc");
                if (dt.Rows.Count <= 0)
                {
                    context.Response.Write("错误！    没有找到id=" + id + "的数据！");
                }
                else if (dt.Rows.Count > 1)
                {
                    context.Response.Write("错误！    找到多条id=" + id + "的数据！");
                }
                else
                {
                    var Data = new { data = dt.Rows[0], data_2 = dt_2.Rows, islogin, id, username = (string)context.Session["username"] };
                    string html = SQLStance.ReturnHtml("detailed.html", Data);
                    context.Response.Write(html);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}