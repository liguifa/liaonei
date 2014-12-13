using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MyBBS
{
    /// <summary>
    /// LogIn 的摘要说明
    /// </summary>
    public class LogIn : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string Username = context.Request["username"];
            string Password = context.Request["password"];
            string Login = context.Request["login"];
            string action = context.Request["action"];
            if (action != "logout")
            {
                if (String.IsNullOrEmpty(Login))//判断是否点击登陆按钮开始
                {
                    context.Response.Redirect("index.ashx");

                }
                else
                {
                    string Md5Psw = FormsAuthentication.HashPasswordForStoringInConfigFile(Password + Username, "MD5");
                    DataTable logIn = SqlHelper.ExecuteDataTable("select * from T_person where username like '" + Username + "'and password  like '" + Md5Psw + "'");
                    if (logIn.Rows.Count == 1)
                    {//login
                        DataTable IsVIP = SqlHelper.ExecuteDataTable("select vip from T_person where username like '" + Username + "'");
                        if (Username == "root")
                        {//判断是否为管理员登入
                            context.Session["username"] = Username;
                            context.Session["IsVip"] = true;
                            //转跳至管理界面
                            context.Response.Redirect("~/HTML/guanli.html");
                        }
                        else if (Convert.ToInt32(IsVIP.Rows[0][0]) == 1)
                        {//检索此用户是否为VIP用户

                            context.Session["username"] = Username;
                            context.Session["IsVip"] = true;
                            context.Response.Redirect("index.ashx");
                        }
                        else
                        {//普通用户登陆

                            context.Session["username"] = Username;
                            context.Session["IsVip"] = false;
                            context.Response.Redirect("index.ashx");

                        }


                    }
                    else if (logIn.Rows.Count == 0)
                    {//用户名或密码错误 
                        context.Response.Write("用户名或密码错误！");
                    }
                    else
                    { //数据库有重名用户 请再次注册或联系管理员
                        context.Response.Write("数据库有重名用户 请再次注册或联系管理员！");
                    }

                }
            }
            else
            {
                context.Session.Clear();
                context.Response.Redirect("index.ashx");
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