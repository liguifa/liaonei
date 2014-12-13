using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MyBBS
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["action"];
            string Username=null;
            bool  state= false;
            if (context.Session["username"] == null)
            {//未登陆
                state = false;
            }
            else
            { //已登录
                //从session中取出基本信息
               
                Username = (string)context.Session["username"];
              //  bool IsVip = (bool)context.Session["IsVip"];
                if (String.IsNullOrEmpty(Username))
                {
                    context.Response.Redirect("index.ashx");
                }
                else
                {    state = true;
             

                }
            }
            if (action == null)
            {
                DataTable dt = SqlHelper.ExecuteDataTable("select top 12 * from Table_3 where VIP like '1' and  Donate <> 1 order by id desc;");
                var data = new  {goods=dt.Rows, Username,state};
                string html = SQLStance.ReturnHtml("start.html", data);
                context.Response.Write(html);
            }
            else if (action == "shop")
            {
                DataTable dt_1 = SqlHelper.ExecuteDataTable("select top 4 * from Table_3 where Classification like '1' and not Donate like '1' order by id desc");
                DataTable dt_2 = SqlHelper.ExecuteDataTable("select top 4 * from Table_3 where Classification like '2' and not Donate like '1' order by id desc");
                DataTable dt_3 = SqlHelper.ExecuteDataTable("select top 4 * from Table_3 where Classification like '3' and not Donate like '1' order by id desc");
                DataTable dt_4 = SqlHelper.ExecuteDataTable("select top 4 * from Table_3 where Classification like '4' and not Donate like '1' order by id desc");
                DataTable dt_5 = SqlHelper.ExecuteDataTable("select top 4 * from Table_3 where Classification like '5' and not Donate like '1' order by id desc");
                DataTable dt_6 = SqlHelper.ExecuteDataTable("select top 4 * from Table_3 where Classification like '6' and not Donate like '1' order by id desc");
                var dt = new { dt1 = dt_1.Rows, dt2 = dt_2.Rows, dt3 = dt_3.Rows, dt4 = dt_4.Rows, dt5 = dt_5.Rows, dt6 = dt_6.Rows ,Username,state};

                string html = SQLStance.ReturnHtml("shop.html", dt);
                context.Response.Write(html);
            }
            else if (action =="user") {
                DataTable dt = SqlHelper.ExecuteDataTable("select top 12 * from Table_3 where VIP like '1' and  Donate <> 1 order by id desc;");
                string html = SQLStance.ReturnHtml("/HTML2/StartUser.html", dt.Rows);
                context.Response.Write(html);
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