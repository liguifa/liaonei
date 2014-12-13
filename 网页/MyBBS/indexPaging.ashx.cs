using MYBBS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MyBBS
{
    /// <summary>
    /// indexPaging 的摘要说明
    /// </summary>
    public class indexPaging : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool isVip =Convert.ToBoolean(context.Request["jp"]);
            string res=context.Request["res"];
            int page =Convert.ToInt32(context.Request["page"]);
            string isTail =Test(page, context, res,isVip);//是否最后一页或第一页  可能取值：-1：第一页；1：最后一页；0：正常；ON 异常;
            if (isTail!= "ON")
            {
                if (res == "s" && isTail == "-1")
                {
                    context.Response.Write("{\"res\":\"-1\"}");
                    return;
                }
                if (res == "x" && isTail == "1")
                {
                    context.Response.Write("{\"res\":\"1\"}");
                    return;
                }
                DataTable dt = null;
                if (res == "x")
                {
                    if (isVip)
                    {
                        dt = SqlHelper.ExecuteDataTable("select rank() over(order by id desc) as hanghao,* into #TEMP from  Table_3 where Donate <> '1';select * FROM #TEMP where hanghao between " + (page * 12 + 1) + " and " + (page + 1) * 12 + ";");
                    }
                    else
                    {
                        dt = SqlHelper.ExecuteDataTable("select rank() over(order by id desc) as hanghao,* into #TEMP from  Table_3 where Donate = '1';select * FROM #TEMP where hanghao between " + (page * 12 + 1) + " and " + (page + 1) * 12 + ";");
                    }
                }
                else
                {
                    if (isVip)
                    {
                        dt = SqlHelper.ExecuteDataTable("select rank() over(order by id desc) as hanghao,* into #TEMP from  Table_3 where VIP like '1' and Donate <> '1';select * FROM #TEMP where hanghao between " + ((page - 2) * 12 + 1) + " and " + ((page-1) * 12) + ";");
                    }
                    else
                    {
                        dt = SqlHelper.ExecuteDataTable("select rank() over(order by id desc) as hanghao,* into #TEMP from  Table_3 where Donate = '1';select * FROM #TEMP where hanghao between " + ((page - 2) * 12 + 1) + " and " + ((page-1) * 12) + ";");
                    }
                }
                string str = "{\"res\":\"OK\",\"fs\":\"" + res + "\",\"tail\":\"" + isTail + "\",\"datanum\":\"" + dt.Rows.Count + "\",\"data\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        str = str + "{\"id\":\"" + dt.Rows[i][1] + "\",\"src\":\"" + dt.Rows[i][2] + "\",\"title\":\"" + dt.Rows[i][3] + "\",\"introduction\":\"" + dt.Rows[i][4] + "\",\"price\":\"" + dt.Rows[i][10] + "\"}";
                    }
                    else
                    {
                        str = str + ",{\"id\":\"" + dt.Rows[i][1] + "\",\"src\":\"" + dt.Rows[i][2] + "\",\"title\":\"" + dt.Rows[i][3] + "\",\"introduction\":\"" + dt.Rows[i][4] + "\",\"price\":\"" + dt.Rows[i][10] + "\"}";
                    }
                }
                str = str + "]}";
                Encoding.UTF8.GetString(Encoding.GetEncoding("GB2312").GetBytes(str));

                context.Response.Write(str);
            }

        }
      
        private string Test(int page, HttpContext context,string res,bool isVip)
        {
            string isTail = "0";//是否最后一页或第一页  可能取值：-1：第一页；1：最后一页；0：正常；ON 请求异常;
            long MaxPage=0;
            if(isVip)
                MaxPage =(int) SqlHelper.ExecuteScalar("select count(*) from Table_3 where VIP='1' and Donate <> '1'");//查询数据库记录数；
            else
                MaxPage = (int)SqlHelper.ExecuteScalar("select count(*) from Table_3 where Donate = '1'");//查询数据库记录数；
            if (MaxPage % 12 == 0)
            {
                MaxPage = MaxPage / 12;
            }
            else
            {
                MaxPage = MaxPage / 12 + 1;
            }
            if (page > MaxPage)
            {
                //请求异常！;
                context.Response.Write("{\"res\":\"0005\"}");
                isTail = "ON";
            }
            else 
            {
                if (res == "s" && page == 0 || res == "x" && page > MaxPage)
                {
                    context.Response.Write("{\"res\":\"0005\"}");
                    isTail = "ON";
                }
            }
            if (isTail != "ON")
            {
                if (page == 1 && res == "s")
                {
                    isTail = "-1";
                }
                else if (page == MaxPage && res=="x")
                {
                    isTail = "1";
                }
                else
                {
                    isTail = "0";
                }
            }
            return isTail;
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