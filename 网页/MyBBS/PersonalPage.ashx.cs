using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MyBBS
{
    /// <summary>
    /// PersonalPage 的摘要说明
    /// </summary>

    public class PersonalPage : IHttpHandler, IRequiresSessionState
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            #region MyRegionn 判断是否登录，如没有登录返回首页;
            string Username = null;
            string Submit = context.Request["submit"];
            //string Submit = context.Request["submitpwd"];
            string Nowpwd = context.Request["NowPwd"];
            string Newpwd = context.Request["NewPwd"];
            string RNewpwd = context.Request["RNewPwd"];
            bool state = false;
            string Email = context.Request["Email"];
            string QQ = context.Request["QQ"];
            string Phone = context.Request["Phone"];
            int College = Convert.ToInt32(context.Request["College"]);
            int Grade = Convert.ToInt32(context.Request["Grade"]);
            if (context.Session["username"] == null)
            {//未登陆
                state = false;
                context.Response.Redirect("index.ashx");

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
            #endregion
            if (state)
            {

                if (Submit == "save")
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery("Update T_Person set  email=@Email, qq=@QQ ,phone=@Phone,college=@College,grade=@Grade where username like'" + Username + "'", new SqlParameter("@Email", Email), new SqlParameter("@Phone", Phone), new SqlParameter("@QQ", QQ), new SqlParameter("@College", College), new SqlParameter("@Grade", Grade));
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Person where username like'" + Username + "'");
                        DataTable dtCollege = SqlHelper.ExecuteDataTable("select * from T_College");
                        DataTable dataCollege = SqlHelper.ExecuteDataTable("select * from T_College where ID like '" + dt.Rows[0][3] + "'");
                        DataTable dtGrade = SqlHelper.ExecuteDataTable("select * from T_Grade");
                        DataTable dataGrade = SqlHelper.ExecuteDataTable("select * from T_Grade where ID like '" + dt.Rows[0][4] + "'");
                        var data = new { College = dtCollege.Rows, Grade = dtGrade.Rows, Email = dt.Rows[0][3], Username = dt.Rows[0][1], dtCollege = dataCollege.Rows[0][1], dtGrade = dataGrade.Rows[0][1], QQ = dt.Rows[0][7], Phone = dt.Rows[0][8], Sex = dt.Rows[0][9] ,pubNum=dt.Rows[0][10]};
                        string html = SQLStance.ReturnHtml("PersonalPage.html", data);
                        context.Response.Write(html);
                    }
                    catch
                    {
                        context.Response.Write("个人中心数据库更新错误，错误代码0002");
                    }
                }
                else if (Submit == "change")
                {
                    if (Newpwd == RNewpwd)
                    {
                        string md5Psw = FormsAuthentication.HashPasswordForStoringInConfigFile(Nowpwd + Username, "MD5");
                        string newmd5Psw = FormsAuthentication.HashPasswordForStoringInConfigFile(Newpwd + Username, "MD5");
                        DataTable logIn = SqlHelper.ExecuteDataTable("select * from T_person where username like '" + Username + "'and password  like '" + md5Psw + "'");
                        if (logIn.Rows.Count == 1)
                        {
                            if (Newpwd.Length >= 6)
                            {
                                try
                                {
                                    SqlHelper.ExecuteNonQuery("Update T_Person set password=@md5Psw where username like'" + Username + "'", new SqlParameter("@md5Psw", newmd5Psw));
                                    context.Response.Write("{\"ok\":\"1\"}");
                                    return;
                                }
                                catch
                                {
                                    context.Response.Write("数据库更新密码写入错误！错误代码0003");
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"ok\":\"2\"}");
                            }
                        }
                        else
                        {
                            context.Response.Write("{\"ok\":\"3\"}");
                        }
                    }
                    else
                    {
                        //TODO
                        context.Response.Write("{\"ok\":\"4\"}");
                    }
                }
                else
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Person where username like'" + Username + "'");
                    DataTable dtCollege = SqlHelper.ExecuteDataTable("select * from T_College");
                    DataTable dataCollege = SqlHelper.ExecuteDataTable("select * from T_College where ID like '" + dt.Rows[0][3] + "'");
                    DataTable dtGrade = SqlHelper.ExecuteDataTable("select * from T_Grade");
                    DataTable dataGrade = SqlHelper.ExecuteDataTable("select * from T_Grade where ID like '" + dt.Rows[0][4] + "'");
                    var data = new { College = dtCollege.Rows, Grade = dtGrade.Rows, Email = dt.Rows[0][3], Username = dt.Rows[0][1], dtCollege = dataCollege.Rows[0][1], dtGrade = dataGrade.Rows[0][1],vip=dt.Rows[0][6], QQ = dt.Rows[0][7], Phone = dt.Rows[0][8], Sex = dt.Rows[0][9],pubNum=dt.Rows[0][10] };
                    string html = SQLStance.ReturnHtml("PersonalPage.html", data);
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