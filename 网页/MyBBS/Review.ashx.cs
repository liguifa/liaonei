using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace MyBBS
{
    /// <summary>
    /// Review 的摘要说明
    /// </summary>
    public class Review : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            long id=Convert.ToInt64(context.Request["ID"]);
            int number = Convert.ToInt32(context.Request["pag"]);
            string status = context.Request["status"];
            
            int temp = (number - 1) * 20;
            DataTable dt = SqlHelper.ExecuteDataTable("select * into #TEMP from  Publish where Goods='"+id+"'and Reply='0' order by ID desc;select top 20 * FROM #TEMP where ID not in (select top " + temp + " ID from #TEMP);");
            DataTable dt2 = SqlHelper.ExecuteDataTable("select * from  Table_3 where id='" + id + "'");
            int isLogin = 0;
            if (dt2.Rows.Count <= 0)
            {
                context.Response.Write("参数ID不存在！");
            }
            else if (dt2.Rows.Count > 1)
            {
                context.Response.Write("参数ID异常！请重试...");
            }
            if (!(context.Session["username"]==null))
            {
                isLogin = 1;
            }
            else
            {
                isLogin = 0;
            }
            
            if (number == 1)
            {
                var data = new { dt = dt.Rows,dt2=dt2.Rows[0],isLogin,number,id};
                string html = SQLStance.ReturnHtml("Review.html", data);
                context.Response.Write(html);
            }
            else
            {
                string str = "{\"request\":\"OK\",\"number\":\"" + number + "\",\"num\":\"" + dt.Rows.Count + "\",\"ID\":\"" + id + "\",\"table\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string temps = Convert.ToString(dt.Rows[i][3]);
                    Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(temps));
                    if (i == 0)
                        str = str + "{\"Username\":\"" + Convert.ToString(dt.Rows[i][2]) + "\",\"Content\":\"" + temps + "\",\"time\":\"" + dt.Rows[i][4] + "\"}";
                    else
                        str = str + ",{\"Username\":\"" + Convert.ToString(dt.Rows[i][2]) + "\",\"Content\":\"" + temps + "\",\"time\":\"" + dt.Rows[i][4] + "\"}";
                }
                str = str + "]}";
                Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));
                context.Response.Write(str);
            }
            if (!String.IsNullOrEmpty(status))
            {
                if (status == "0")
                {   
                    SqlHelper.ExecuteNonQuery("Update Publish set Status='0' where ID like '" + context.Request["msgId"] + "'");
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