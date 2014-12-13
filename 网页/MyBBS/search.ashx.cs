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
    /// search 的摘要说明
    /// </summary>
    public class search : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["action"];
            string search = context.Request["search"];
            string page =context.Request["page"];
            string Username = null;
            bool state = false;
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
            int temp =(Convert.ToInt32(page)-1) * 40;
            if (search == "true")
            {
                string str = action;
                int[] p =new int[2];
                action = "%" + action + "%";
                p=SqlHelper.GetCount(action, "Title");
                DataTable dt_temp = SqlHelper.ExecuteDataTable("select rank() over(order by id desc) as hanghao,* into #TEMP from  Table_3 where Title like '"+action+"' and Donate <> '1';select * FROM #TEMP where hanghao between " + (temp+1) + " and " + (temp+40) + ";");
                var dt = new { p1=p[0], dt = dt_temp.Rows, page, search, str, p2 =p[1] ,Username,state };
                string html = SQLStance.ReturnHtml("search.html", dt);
                context.Response.Write(html);
            }
            else if (search == "false")
            {
                string str;
                switch (action)
                {
                    case "1":
                        {
                            str = "书籍类";
                            break;
                        }
                    case "2":
                        {
                            str = "数码类";
                            break;
                        }
                    case "3":
                        {
                            str = "电器类";
                            break;
                        }
                    case "4":
                        {
                            str = "配饰类";
                            break;
                        }
                    case "5":
                        {
                            str = "服饰类";
                            break;
                        }
                    case "6":
                        {
                            str = "其他类";
                            break;
                        }
                    default: { str = ""; break; }

                }
                int[] p = new int[2];
                p = SqlHelper.GetCount(action, "Classification");
                DataTable dt_temp = SqlHelper.ExecuteDataTable("select * into #TEMP from  Table_3 where Classification like '" + action + "' and Donate <> '1' order by id desc;select top 40 * FROM #TEMP where id not in (select top " + temp + " id from #TEMP);");
                var dt = new { p1 = p[0], dt = dt_temp.Rows, page, search, str, p2 = p[1] };
                string html = SQLStance.ReturnHtml("search.html", dt);
                context.Response.Write(html);
            }
            else
            {
                context.Response.Write("action参数错误");
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