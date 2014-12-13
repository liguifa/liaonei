using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;



namespace MyBBS
{
    /// <summary>
    /// Publish 的摘要说明
    /// </summary>
    public class Publish : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string request = "";
            int req = 1;
            req = Convert.ToInt32( context.Request["req"]);
            try { request = context.Request["request"]; }
            catch { context.Response.Write("{\"request\":\"ON\"}"); return; }
            switch (req)
            {
                case 1:
                    {
                        if (string.IsNullOrEmpty(request))
                        {
                            context.Response.Write("{\"request\":\"Warning\"}");
                            return;
                        }
                        string username=(string)context.Session["username"];
                        long id=Convert.ToInt64(context.Request["ID"]);
                        string reply = "0";
                        string status = "1";
                        DateTime now = DateTime.Now;
                        string time = DoubleTime(now.Year.ToString().Trim()) + "-" + DoubleTime(now.Month.ToString().Trim()) + "-" + DoubleTime(now.Day.ToString().Trim()) + " " + DoubleTime(now.Hour.ToString().Trim()) + ":" + DoubleTime(now.Minute.ToString()) + ":" + DoubleTime(now.Second.ToString());
                        string BeReplyUsername = SqlHelper.ExecuteDataTable("select Username from Table_3 where id='" + id + "'").Rows[0][0].ToString(); 
                        SqlHelper.ExecuteNonQuery("Insert into Publish (Goods,Username,Content,Time,Reply,Status,beRepliedUsername) values(@Goods,@Username,@Content,@Time,@Reply,@Status,@beRepliedUsername)"
                                         , new SqlParameter("@Goods", id)
                                         , new SqlParameter("@Username", username)
                                         , new SqlParameter("@Content", request)
                                         , new SqlParameter("@Time", time)
                                         , new SqlParameter("@Reply", reply)
                                         , new SqlParameter("@Status", status)
                                         , new SqlParameter("@beRepliedUsername", BeReplyUsername));
                        string str = "{\"Handle\":\"Review\",\"request\":\"OK\",\"time\":\"" + time + "\",\"username\":\"" + username + "\"}";
                        Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));
                        context.Response.Write(str);
                        break;
                    }
                case 2:
                    {
                        string id = context.Request["ID"];
                        DataTable dt = SqlHelper.ExecuteDataTable("select Username,Content from Publish where Reply='" + id + "'");
                        string str ="{\"Handle\":\"Reply\",\"request\":\"OK\",\"number\":\"" + dt.Rows.Count + "\",\"table\":[";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                                if(i==0)
                                    str = str + "{\"Username\":\"" + dt.Rows[i][0] + "\",\"Content\":\"" + dt.Rows[i][1] + "\"}";
                                else
                                    str = str + ",{\"Username\":\"" + dt.Rows[i][0] + "\",\"Content\":\"" + dt.Rows[i][1] + "\"}";
                        }
                        str = str + "]}";


                        //汉字转为Unicode编码：
                        Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));
                        context.Response.Write(str);
                        break;
                    }
                case 3:
                    {
                        if (String.IsNullOrEmpty(request))
                        {
                            context.Response.Write("{\"request\":\"Warning\"}");
                            return;
                        }
                        string username = (string)context.Session["username"];
                        long id = Convert.ToInt64(context.Request["ID"]);
                        string Goods=context.Request["GoodID"];
                        string status = "1";
                        DateTime now = DateTime.Now;
                        string time = DoubleTime(now.Year.ToString().Trim()) + "-" + DoubleTime(now.Month.ToString().Trim()) + "-" + DoubleTime(now.Day.ToString().Trim()) + " " + DoubleTime(now.Hour.ToString().Trim()) + ":" + DoubleTime(now.Minute.ToString()) + ":" + DoubleTime(now.Second.ToString());
                        string BeReplyUsername = SqlHelper.ExecuteDataTable("select Username from Publish where id='" + id + "'").Rows[0][0].ToString();
                        SqlHelper.ExecuteNonQuery("Insert into Publish (Goods,Username,Content,Time,Reply,Status,beRepliedUsername) values(@Goods,@Username,@Content,@Time,@Reply,@Status,@beRepliedUsername)"
                                         , new SqlParameter("@Goods", Goods)
                                         , new SqlParameter("@Username", username)
                                         , new SqlParameter("@Content", request)
                                         , new SqlParameter("@Time", time)
                                         , new SqlParameter("@Reply", id)
                                         , new SqlParameter("@Status", status)
                                         ,new SqlParameter("@beRepliedUsername",BeReplyUsername));
                        string str = "{\"Handle\":\"ReplyQT\",\"request\":\"OK\",\"username\":\"" + username + "\"}";
                        Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));
                        context.Response.Write(str);
                        break;
                    }
                default: context.Response.Write("{\"request\":\"QT\"}"); break;
            }
        }
        public string DoubleTime(string time)
        {
            string DoubleT = null;
            if (time.Length < 2)
                DoubleT = "0" + time;
            else
                DoubleT = time;
            return DoubleT;
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