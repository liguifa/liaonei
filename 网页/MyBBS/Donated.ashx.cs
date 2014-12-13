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
    /// Donated 的摘要说明
    /// </summary>
    public class Donated : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
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
                {
                    state = true;


                }
            }
            

            DataTable dt = SqlHelper.ExecuteDataTable("select top 12 * from Table_3 where Donate='1' order by id desc");
            var data = new { goods = dt.Rows, Username, state };
            string html = SQLStance.ReturnHtml("Donated.html", data);
            context.Response.Write(html);
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