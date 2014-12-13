using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MyBBS.HTML.admin
{
    /// <summary>
    /// admin 的摘要说明
    /// </summary>
    public class admin : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string request = context.Request["request"];
            switch (request)
            {
                case "Login": Login(context); break;
                case "Goods": if (isLogin(context)) { Data(context,"Table_3","Goods"); } else { context.Response.Write("你没有权限访问！"); } break;
                case "Person": if (isLogin(context)) { Data(context,"T_Person","Person"); } else { context.Response.Write("你没有权限访问！"); } break;
                case "Publish": if (isLogin(context)) { Data(context, "Publish", "Publish"); } else { context.Response.Write("你没有权限访问！"); } break;
                case "Delete": 
                    if (isLogin(context)) 
                    {
                        if (Delete(context))
                        {
                            context.Response.Write("{\"res\":\"OK\",\"Operation\":\"DEL\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"res\":\"NO\",\"Operation\":\"DEL\"}");
                        }
                    } 
                    else 
                    { 
                        context.Response.Write("你没有权限访问！"); 
                    }
                    break;
                case "Edit":
                    if (isLogin(context))
                    {
                        if (Edit(context))
                        {
                            context.Response.Write("{\"res\":\"OK\",\"Operation\":\"EDIT\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"res\":\"NO\",\"Operation\":\"EDIT\"}");
                        }
                    }
                    else
                    {
                        context.Response.Write("你没有权限访问！"); 
                    }
                    break;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool Login(HttpContext context)
        {
            string adminname = context.Request["amdin"];
            string pass = context.Request["adminpassword"];
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(pass + adminname, "MD5");
            if (pass == "" || adminname == "")
            {
                context.Response.Write("密码或用户名为空！");
            }
            else
            {
                DataTable data = SqlHelper.ExecuteDataTable("select vip from T_Person where username='" + adminname + "' and password='" + password + "'");
                try
                {
                    if (Convert.ToInt32(data.Rows[0][0]) == -1)
                    {
                        context.Session["admin"] = adminname;
                        context.Response.Write("<!DOCTYPE html><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/><title></title></head><body><ul><li><a href=\"admin.ashx?request=Person\">用户表</a></li><li><a href=\"admin.ashx?request=Goods\">商品表</a></li><li><a href=\"admin.ashx?request=Publish\">评论表</a></li><li><a href=\"#\">其他表</a></li></ul></body></html>");
                    }
                    else
                    {
                        context.Response.Write("你不是管理员!");
                    }
                }
                catch 
                {
                    context.Response.Write("你不是管理员!");
                }
            }
            return true;
        }
        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool Data(HttpContext context,string table,string html_name)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select* from "+table);
            string html = SQLStance.ReturnHtml("/admin/admin"+html_name+".html",dt.Rows);
            context.Response.Write(html);
            return true;
        }
        /// <summary>
        /// 删数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool Delete(HttpContext context)
        {
            long id = Convert.ToInt64(context.Request["ID"]);
            string table = context.Request["table"];
            try
            {
                int i = 0;
                i = SqlHelper.ExecuteNonQuery("delete from "+table+" where id='"+id+"'");
                if (i <= 0)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        /// <summary>
        /// 改数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool Edit(HttpContext context)
        {
            string table=context.Request["table"];
            long id=Convert.ToInt64(context.Request["ID"]);
            int num = Convert.ToInt32(context.Request["num"]);
            string[] data = new string[num];
            for (int n = 0; n < num; n++)
            {
                data[n] = context.Request["data" + n];
            }
            if (table == "T_Person")
            {
                 data[1]= FormsAuthentication.HashPasswordForStoringInConfigFile(data[1] + data[0], "MD5");
            }
                try
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from " + table + " where id='" + id + "'");
                    string[] str = new string[dt.Columns.Count];
                    int i=0;
                    foreach (DataColumn c in dt.Columns)
                    {
                        str[i++] =c.ColumnName;
                    }
                    string SQL = "update "+table+" set ";
                    for (int n = 1; n < num; n++)
                    {
                        if (n == 1)
                        {
                            SQL = SQL + str[n] + " = '" + data[n-1] + "'";
                        }
                        else
                        {
                            SQL = SQL +","+ str[n] + " = '" + data[n-1]+"'";
                        }
                    }
                    SQL = SQL + " where id='" + id + "'";
                    int y = 0;
                    y = SqlHelper.ExecuteNonQuery(SQL);
                    if (y <= 0)
                    {
                        return false;
                    }
                    return true;

                }
                catch
                {
                    return false;
                }
        }
        /// <summary>
        /// 增数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool Add(HttpContext context)
        {
            return true;
        }
        private bool isLogin(HttpContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(context.Session["admin"].ToString()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch 
            {
                context.Response.Write("登录时间过期！请重新登录...");
                return false;
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