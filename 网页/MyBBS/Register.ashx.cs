using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
namespace MyBBS
{
    /// <summary>
    /// Register 的摘要说明
    /// </summary>
    public class Register : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["action"];
            int vip = 0;
            if (action == "AddNew")
            {
                //判断是否含有Save并且等于true，如果是的话就说明是点击【保存】按钮请求来的
                bool save = Convert.ToBoolean(context.Request["Save"]);
                if (save){//是保存
                        #region MyRegion 申明接收
                    string name = context.Request["Name"];
                    string qq=context.Request["QQ"];
                    string phone = context.Request["Phone"];
                    string pwd = context.Request["Pwd"];
                    bool evrythingallright = true;
                    string Rpwd = context.Request["Rpwd"];
                    string email = context.Request["Email"];
                    int college = Convert.ToInt32(context.Request["College"]);
                    int grade = Convert.ToInt32(context.Request["Grade"]);
                    string sex =context.Request["sex"]; 
                    #endregion
                        #region MyRegionMD5加盐加密

                    string Md5pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd + name, "MD5");
                    
                    #endregion
                        DataTable dt = SqlHelper.ExecuteDataTable("select * from T_person where username like '" + name + "'");
                        if (action == "AddNew"){
                            #region  my判断是否为合法输入
		
                            if(name.Length==0)
                            {
                                evrythingallright = false;
                            }
                           
                            for (int i = 0; i < name.Length; i++)
                            {

                                if (!((name[i] >='a' && name[i] <='z') || (name[i] >='A' && name[i] <= 'Z') || (name[i] <= '9' && name[i] >= '0')))
                                {
                                    evrythingallright = false;
                                    break;
                                }
                            } 
	                        #endregion
                            evrythingallright = evrythingallright && (dt.Rows.Count == 0);
                            if (evrythingallright){
                                if ((pwd != Rpwd) || (pwd.Length < 6)){
                                    #region MyRegion 向前台传入注册密码不一致或少于六位
                                    DataTable dtCollege = SqlHelper.ExecuteDataTable("select * from T_College");
                                    DataTable dtGrade = SqlHelper.ExecuteDataTable("select * from T_Grade");
                                    var data = new { College = dtCollege.Rows, Grade = dtGrade.Rows, Name = "密码不一致或少于六位！", Email = "@qq.com" };

                                    string html = SQLStance.ReturnHtml("Register.html", data);

                                    context.Response.Write(html);
                                    #endregion
                                }
                                else{
                                    #region MyRegion 成功！新建数据注册成功！
                                    try
                                    {
                                        SqlHelper.ExecuteNonQuery("Insert into T_Person(username,password,email,college,grade,vip,qq,phone,sex,pubNum) values(@username,@password,@email,@college,@grade,@vip,@qq,@phone,@sex,@pubNum)"
                                                                      , new SqlParameter("@username", name)
                                                                      , new SqlParameter("@password", Md5pwd)
                                                                      , new SqlParameter("@email", email)
                                                                      , new SqlParameter("@college", college)
                                                                      , new SqlParameter("@grade", grade)
                                                                       , new SqlParameter("@vip", vip)
                                                                        , new SqlParameter("@qq", qq)
                                                                        ,  new SqlParameter("@phone",phone)
                                                                        , new SqlParameter("@sex", sex)
                                                                          , new SqlParameter("@pubNum", "0"));

                                        context.Response.Write("<!DOCTYPE html><html ><head><meta http-equiv=\"Refresh\" content=\"2;URL=index.ashx\" /></head><body><h1>注册成功！即将跳转至首页</h1></body></html> ");
                                    }
                                    catch {
                                        context.Response.Write("数据库写入错误 错误代码0001");
                                    }
                                    #endregion
                                }
                            }
                            else if (dt.Rows.Count > 0)
                            {
                                #region MyRegion 验证失败向前台传入注册 此用户名已被注册
                                DataTable dtCollege = SqlHelper.ExecuteDataTable("select * from T_College");
                                DataTable dtGrade = SqlHelper.ExecuteDataTable("select * from T_Grade");
                                var data = new { College = dtCollege.Rows, Grade = dtGrade.Rows, Name = "此用户名已被注册！", Email = email };

                                string html = SQLStance.ReturnHtml("Register.html", data);
                                context.Response.Write(html);  
                                #endregion
                            }
                            else
                            {
                                #region MyRegion 验证失败向前台传入注册所需数据 非法的用户名
                                DataTable dtCollege = SqlHelper.ExecuteDataTable("select * from T_College");
                                DataTable dtGrade = SqlHelper.ExecuteDataTable("select * from T_Grade");
                                var data = new { College = dtCollege.Rows, Grade = dtGrade.Rows, Name = "非法的用户名！", Email = email };

                                string html = SQLStance.ReturnHtml("Register.html", data);
                                context.Response.Write(html);  
                                #endregion
                            }
                        }
                }
                else{
                    #region MyRegion验证失败 向前台传入注册所需数据
                    DataTable dtCollege = SqlHelper.ExecuteDataTable("select * from T_College");
                    DataTable dtGrade = SqlHelper.ExecuteDataTable("select * from T_Grade");
                    var data = new { College = dtCollege.Rows, Grade = dtGrade.Rows, Name = "只能输入字母和数字!", Email = "@qq.com" };

                    string html = SQLStance.ReturnHtml("Register.html", data);

                    context.Response.Write(html); 
                    #endregion
                }
            }
            else if (action == "Edit")
            {
            }
            else
            {
                context.Response.Write("Action参数错误！");
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