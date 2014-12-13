using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MyBBS
{
    /// <summary>
    /// personalPage_SH 的摘要说明
    /// </summary>
    public class personalPage_SH : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string ispostback = context.Request["save"];
            string _productName = context.Request["productName"];
            string _price = "0";
           _price = context.Request["price"];
            string _introduction = context.Request["introduction"];
            string _detalis = context.Request["details"];
            HttpPostedFile productImg = context.Request.Files["ProductImage"];
            int _detailed =Convert.ToInt32( context.Request["detailed"]);
            string username = null;
            string filename = null;
            int price=0;
            string classification = context.Request["classification"];
            if (context.Session["username"] == null)
            {//未登陆
                context.Response.Write("<script>你还没有登录哦！</script>");
                context.Response.Redirect("javascript:history.go(-1)");

            }
            else
            { //已登录
                //从session中取出基本信息

                username = (string)context.Session["username"];
                
                if (String.IsNullOrEmpty(username))
                {
                    context.Response.Redirect("index.ashx");
                }
                
            }
            
            if (ispostback ==null)
            {
                var data = new {Username=username };
                string html = SQLStance.ReturnHtml("personalPage_SH.html", data);
                context.Response.Write(html);
                
            }
            else if (ispostback == "save")
            {
                try//价格合法性判断
                {
                    price = Convert.ToInt32(_price);
                }
                catch
                {
                    string html = SQLStance.ReturnHtml("personalPage_SH.html", 0);
                    context.Response.Write(html);
                    context.Response.Write("<script> alert(\"价格只能是数字！\")</script>");
                    return;


                }
                if (_detailed == 1)//捐赠物品 价格为0；
                {
                    price = 0;
                }
                if (String.IsNullOrEmpty(_productName) || String.IsNullOrEmpty(_introduction) || String.IsNullOrEmpty(_detalis))
                {

                    context.Response.Write("<script> alert(\"上传页面不允许有空值！\")</script>");
                    var data = new {  Username = username,  };
                    string html = SQLStance.ReturnHtml("personalPage_SH.html", data);
                    context.Response.Write(html); return;
                }
                else
                {
                    try
                    {
                        if (productImg.ContentType == "image/jpeg" || productImg.ContentType == "image/png" || productImg.ContentType == "image/x-png" || productImg.ContentType == "image/gif")
                        {

                            filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(productImg.FileName);
                            productImg.SaveAs(context.Server.MapPath("~/uploadfile/" + filename));
                        }
                        else
                        {
                            context.Response.Write("<script> alert(\"图片格式错误\")</script>");
                            string html = SQLStance.ReturnHtml("personalPage_SH.html", 1);
                            context.Response.Write(html); return;
                        }
                    }
                    catch
                    {
                        context.Response.Write("<script> alert(\"文件不能为空或未知错误！\")</script>");
                        return;
                    }

                }

                try
                {
                    DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Person where username like'" + username + "'");
                    if (dt.Rows.Count == 1)
                    {
                        int vip = Convert.ToInt32(dt.Rows[0][6]);
                        int pubNum = Convert.ToInt32(dt.Rows[0][10]);
                        pubNum++;
                        if (pubNum >= 5)
                        {
                            vip = 1;
                        }
                        string QQ = Convert.ToString(dt.Rows[0][7]);
                        string Phone = Convert.ToString(dt.Rows[0][8]);
                        string time = DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.ToString("MM") + "月" + DateTime.Now.ToString("dd") + "日";
                        string src = "uploadfile/"+filename;
                        SqlHelper.ExecuteNonQuery("Insert into Table_3(SRC,Title,Introduction,Details,Username,Phone,QQ,Time,Price,VIP,Classification,Donate) values ('" + src + "','" + _productName + "','" + _introduction + "','" + _detalis + "','" + username + "','" + Phone + "','" + QQ + "','" + time + "','" + price + "','" + vip + "','" + classification + "','" + _detailed + "')");
                        SqlHelper.ExecuteNonQuery("Update T_Person set pubNum='" + pubNum + "' ,vip='" + vip + "' where Username like '" + username + "'");
                        context.Response.Write("<script> alert(\"发布成功！\")</script>");
                        DataTable dt2 = SqlHelper.ExecuteDataTable("select id from Table_3 where SRC like'" + src + "'");
                        
                        context.Response.Redirect("~/detailed.ashx?ID="+dt2.Rows[0][0]+"&action=detailed");
                        return;
                    }
                    else
                    {
                        context.Response.Write("用户名错误！多行数据");
                    }
                }
                catch
                {
                    context.Response.Write("<script> alert(\"数据库插入错误！ 错误代码：\")</script>");
                    return;
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