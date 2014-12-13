using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace MyBBS.personalPage
{
    /// <summary>
    /// persoalMassage 个人消息
    /// </summary>
    public class persoalMassage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            /*
             *   当URL中pageNum 为空值时，只显示第一页（0、1）
             *   当URL中pageNum 存在时，显示第pageNum页
             *   每页五条消息，未读消息在前台背景标为红色；
             *   Ajax动态删除消息；
             *   json回调。
             *   删除物品时，删除所有相关消息(不考虑其他因素)
             *   
             */
            context.Response.ContentType = "text/html";

            string msgId = context.Request["id"];
            string Username = null;
            string isDel = context.Request["isDel"];
            if (context.Session["username"] == null)
            {//未登陆

                context.Response.Redirect("index.ashx");

            }
            else
            { //已登录
                //从session中取出基本信息

                Username = (string)context.Session["username"];
            }
            if (!String.IsNullOrEmpty(isDel))
            {
                string msgid = context.Request["msgId"];
                try
                {
                    SqlHelper.ExecuteNonQuery("delete  from Publish where ID like'" + msgid +  "'or Reply like'"+msgid+"'");
                    context.Response.Write("{\"back\":\"ok\"}");
                }
                catch
                {
                    context.Response.Write("{\"back\":\"no\"}");
                }
                return;
            }
            int pageNum = 0;
            if (context.Request["pageNum"] != null)
            {
                pageNum = Convert.ToInt32(context.Request["pageNum"]);

            }
            

            int totalMsg = (int)SqlHelper.ExecuteScalar("select count(*) from Publish where (beRepliedUsername like '" + Username + "')and(Username <>'"+Username+"')");
            int pageCount = (int)Math.Ceiling(totalMsg / 10.0);
            if (pageNum > pageCount)
            {
                //没有更多消息
                context.Response.Write("{\"request\":\"no\"}");
            }
            else if (pageNum ==0)
            {
                pageNum++;
                DataTable msg = SqlHelper.ExecuteDataTable(@" select * from
                                                            (select p.Id as ID, p.Username as Username ,p.Content as Content,p.Time as Time ,p.Reply as Reply,p.Status as Status,p.Goods as Goods,p.beRepliedUsername as beRepliedUsername,
                                                            row_number() over (order by p.Id desc) as num 
                                                            from Publish p where  (beRepliedUsername like '" + Username + "')and(Username <>'"+Username+"')) as s where s.num between @Start and @End;",
                                                                 new SqlParameter("@Start", (pageNum - 1) * 10 + 1),
                                                                 new SqlParameter("@End", pageNum * 10));
                var data = new { msg = msg.Rows, Username ,pageNum};
                string html = SQLStance.ReturnHtml("personalMessage.html", data);
                context.Response.Write(html);
            }
            else
            {
                pageNum++;
                DataTable msg = SqlHelper.ExecuteDataTable(@" select * from
                                                            (select p.Id as ID, p.Username as Username ,p.Content as Content,p.Time as Time ,p.Reply as Reply,p.Status as Status,p.Goods as Goods,p.beRepliedUsername as beRepliedUsername,
                                                            row_number() over (order by p.Id desc) as num 
                                                            from Publish p where ( beRepliedUsername like '" + Username + "')and(Username <>'" + Username + "')) as s where s.num between @Start and @End;",
                                                              new SqlParameter("@Start", (pageNum - 1) * 10 + 1),
                                                              new SqlParameter("@End", pageNum * 10));

                string str = "{\"request\":\"OK\",\"pageNum\":\"" + pageNum + "\",\"num\":\"" + msg.Rows.Count + "\",\"table\":[";

                for (int i = 0; i < msg.Rows.Count; i++)
                {
                    DataTable goods = SqlHelper.ExecuteDataTable("select Title from Table_3 where id like'" + msg.Rows[i][6] + "'");
                    if (i == 0)
                        str = str + "{\"Username\":\"" + Convert.ToString(msg.Rows[i][1]) + "\",\"msgId\":\"" + Convert.ToString(msg.Rows[i][0]) + "\",\"GoodsId\":\"" + Convert.ToString(msg.Rows[i][6]) + "\",\"Goods\":\"" + Convert.ToString(goods.Rows[0][0]) + "\",\"Content\":\"" + msg.Rows[i][2] + "\",\"time\":\"" + msg.Rows[i][3] + "\",\"status\":\"" + Convert.ToString(msg.Rows[i][5]) + "\"}";
                    else
                        str = str + ",{\"Username\":\"" + Convert.ToString(msg.Rows[i][1]) + "\",\"msgId\":\"" + Convert.ToString(msg.Rows[i][0]) + "\",\"GoodsId\":\"" + Convert.ToString(msg.Rows[i][6]) + "\",\"Goods\":\"" + Convert.ToString(goods.Rows[0][0]) + "\",\"Content\":\"" + msg.Rows[i][2] + "\",\"time\":\"" + msg.Rows[i][3] + "\",\"status\":\"" + Convert.ToString(msg.Rows[i][5]) + "\"}";
                }
                str = str + "]}";
                Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));
                context.Response.Write(str);
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