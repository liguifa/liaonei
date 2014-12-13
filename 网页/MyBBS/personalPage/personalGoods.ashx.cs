using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MyBBS.personalPage
{
    /// <summary>
    /// personalGoods 的摘要说明
    /// </summary>
    public class personalGoods : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string username = null;
            string ID = context.Request["submit"];
            string title = context.Request["title"];
            string introduction = context.Request["introduction"];
            string details = context.Request["details"];
            string phone = context.Request["phone"];
            string qq = context.Request["qq"];
            string price = context.Request["price"];
            long id = 0;
            int have = 0;
            string isDel =context.Request["isDel"];
         
            
            if (context.Session["username"] == null)
            {//未登陆

                context.Response.Redirect("index.ashx");

            } if (!String.IsNullOrEmpty(isDel))
            {
                string goodsId = context.Request["goodsId"];
                try
                {   //删除物品相关信息
                    DataTable dt = SqlHelper.ExecuteDataTable("select SRC from Table_3 where id like '"+goodsId+"'");
                    SqlHelper.ExecuteNonQuery("delete  from Table_3 where id like'" + goodsId + "'");
                    SqlHelper.ExecuteNonQuery("delete  from Publish where Goods like'" + goodsId + "'");
                    File.Delete(context.Server.MapPath("~/") + dt.Rows[0][0]);
                    context.Response.Write("{\"back\":\"ok\"}");
                }
                catch
                {
                    context.Response.Write("{\"back\":\"no\"}");
                }
                return;
            }
            else
            { //已登录
                //从session中取出基本信息

                username = (string)context.Session["username"];

                if (String.IsNullOrEmpty(username))
                {
                    context.Response.Redirect("javascript:history.go(-1)");
                }
                if (String.IsNullOrEmpty(ID))//用户未修改
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from Table_3 where username like'" + username + "'");
                    if (dt.Rows.Count < 1)
                    {
                        have = 0;
                        var data = new { Username = username, have };
                        string html = SQLStance.ReturnHtml("personalGoods.html", data);
                        context.Response.Write(html);
                        return;
                    }
                    else
                    {
                        have = 1;
                        var data = new { dt = dt.Rows, Username = username, have };
                        string html = SQLStance.ReturnHtml("personalGoods.html", data);
                        context.Response.Write(html); return;

                    }
                }
                else
                {
                   
                   
                   

                        if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(introduction) || String.IsNullOrEmpty(details) || String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(qq) || String.IsNullOrEmpty(price))
                        {
                            DataTable dt = SqlHelper.ExecuteDataTable("select * from Table_3 where username like'" + username + "'");
                            if (dt.Rows.Count < 1)
                            {
                                have = 0;
                                var data = new { Username = username, have };
                                string html = SQLStance.ReturnHtml("personalGoods.html", data);
                                context.Response.Write(html);

                            }
                            else
                            {
                                have = 1;
                                var data = new { dt = dt.Rows, Username = username, have };
                                string html = SQLStance.ReturnHtml("personalGoods.html", data);
                                context.Response.Write(html);
                            }
                            context.Response.Write("<script> alert(\"不允许有空值，修改失败！\")</script>"); return;

                        }
                        try
                        {
                            id = Convert.ToInt64(ID);
                        }
                        catch
                        {
                            DataTable dt = SqlHelper.ExecuteDataTable("select * from Table_3 where username like'" + username + "'");
                            if (dt.Rows.Count < 1)
                            {
                                have = 0;
                                var data = new { Username = username, have };
                                string html = SQLStance.ReturnHtml("personalGoods.html", data);
                                context.Response.Write(html);  

                            }
                            else
                            {
                                have = 1;
                                var data = new { dt = dt.Rows, Username = username, have };
                                string html = SQLStance.ReturnHtml("personalGoods.html", data);
                                context.Response.Write(html);
                            }
                            context.Response.Write("<script> alert(\"非法的价格输入，修改失败！\")</script>"); return;
                        }
                    }
                    try//修改数据
                    {
                        SqlHelper.ExecuteNonQuery("Update Table_3 set Title=@title,Introduction=@idtroduction,Details=@details,Phone=@phone,QQ=@qq,Price=@price where id like'" + ID + "'", new SqlParameter("@title", title), new SqlParameter("@idtroduction", introduction), new SqlParameter("@details", details), new SqlParameter("@phone", phone), new SqlParameter("@qq", qq), new SqlParameter("@price", price));
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from Table_3 where username like'" + username + "'");
                        if (dt.Rows.Count < 1)
                        {
                            have = 0;
                            var data = new { Username = username, have };
                            string html = SQLStance.ReturnHtml("personalGoods.html", data);
                            context.Response.Write(html);
                            return;
                        }
                        else
                        {
                            have = 1;
                            var data = new { dt = dt.Rows, Username = username, have };
                            string html = SQLStance.ReturnHtml("personalGoods.html", data);
                            context.Response.Write(html); return;

                        }
                        context.Response.Write("<script> alert(\"修改成功！\")</script>"); return;
                    }
                    catch
                    {
                        context.Response.Write("<script> alert(\"更新数据错误！错误代码0006\")</script>"); return;
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