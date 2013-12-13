using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using Vortaro.Controllers.DAL;
using VortaroModel;
using System.Data;
using Newtonsoft.Json;

namespace Vortaro.Controllers
{
    [Authorize]
    [HandleError]
    public class ColumnController : Controller
    {
        //获取列字段信息
        public void GetPageColumn()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string page = Params["page"];
            string rows = Params["rows"];
            int intPage = int.Parse((page == null || page == "0") ? "1" : page);
            //每页显示条数  
            int pageSize = int.Parse((rows == null || rows == "0") ? "10" : rows);
            //每页的开始记录  第一页为1  第二页为number +1   
            int start = (intPage - 1) * pageSize;
            string tablesCode = Params["tablesCode"];
            Response.Write(DColumn.GetPageColumn(start, pageSize, "", tablesCode));
            Response.End();
        }
        //保存列字段信息
        public void SaveColumn()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Column column = new Column();
            column.Author = User.Identity.Name;
            column.TablesCode = new Guid(Params["tablesCode"]);
            column.Owner = Params["owner"];
            column.Name = Params["name"];
            column.Type = Params["type"];
            column.Bewrite = Params["bewrite"];
            column.FieldState = int.Parse(Params["fieldState"]);
            column.HideAuthor = Params["hideAuthor"];
            column.HideTime = DateTime.Now;//默认为空
            string result = string.Empty;
            if (Params["code"] != null)
            {
                column.Code = new Guid(Params["code"]);
                result = DColumn.Update(column) != null ? "{HasError:false,msg:'列字段编辑成功！'}" : "{HasError:true,msg:'列字段编辑失败，请稍候再试！'}";
            }
            else
            {
                column.Code = Guid.NewGuid();
                result = DColumn.Add(column) != null ? "{HasError:false,msg:'列字段创建成功！'}" : "{HasError:true,msg:'列字段创建失败，请稍候再试！'}";
            }
            Response.Write(result);
            Response.End();
        }
        //批量保存列字段说明
        public void SaveColumnRemark() 
        {
            NameValueCollection Params = HttpContext.Request.Params;//参数
            List<Column> Columnlist = JsonConvert.DeserializeObject<List<Column>>(Params["changRecord"]);
            Response.Write(DColumn.SaveColumnRemark(Columnlist) > 0 ? "{HasError:false,msg:'列字段说明保存成功！'}" : "{HasError:true,msg:'列字段说明保存失败！'}");
            Response.End();
        }
        //删除列字段信息
        public void DeleteColumn()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string result = DColumn.Delete(new Guid(Params["code"])) != null ? "{HasError:false,msg:'列字段删除成功！'}" : "{HasError:true,msg:'列字段删除失败，请稍候再试！'}";
            Response.Write(result);
            Response.End();
        }
        //获取列字段类型
        public void GetSQL2008ColumTypes() 
        {
            string Types = @"[{'id':1,'text':'bigint'},
                {'id':2,'text':'binary(50)'},
                {'id':3,'text':'bit'},
                {'id':4,'text':'char(10)'},
                {'id':5,'text':'date'},
                {'id':6,'text':'datetime'},
                {'id':7,'text':'datetime2(7)'},
                {'id':8,'text':'datetimeoffset(7)'},
                {'id':9,'text':'decimal(18,0)'},
                {'id':10,'text':'float'},
                {'id':11,'text':'geography'},
                {'id':12,'text':'geometry'},
                {'id':13,'text':'image'},
                {'id':14,'text':'int'},
                {'id':15,'text':'money'},
                {'id':16,'text':'nchar(10)'},
                {'id':17,'text':'ntext'},
                {'id':18,'text':'numeric(18,0)'},
                {'id':19,'text':'nvarchar(50)'},
                {'id':20,'text':'nvarchar(MAX)'},
                {'id':21,'text':'reat'},
                {'id':22,'text':'smalldatetime'},
                {'id':23,'text':'smallmoney'},
                {'id':24,'text':'sql_variant'},
                {'id':25,'text':'text'},
                {'id':26,'text':'time(7)'},
                {'id':27,'text':'timestamp'},
                {'id':28,'text':'tinyint'},
                {'id':29,'text':'uniqueidentifier'},
                {'id':30,'text':'varbinary(50)'},
                {'id':31,'text':'varbinary(MAX)'},
                {'id':32,'text':'varchar(50)'},
                {'id':33,'text':'varchar(MAX)'},
                {'id':34,'text':'xml'}]";
            Response.Write(Types.Replace("'","\""));
            Response.End();
        }
        //同步列字段及说明信息
        public void SynchronousColumnBewrite() 
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string ServerName = Params["ServerName"];//服务器名称
            string ServerUser = Params["ServerUser"];//用户名
            string ServerPwd = Params["ServerPwd"];//密码
            ServerPwd = NHibernateHelper.DecryptAES(ServerPwd, "zhangzhangdebing");//解密
            string DatabaseName = Params["DatabaseName"];//指定数据库名称
            string[] TablesCode = Params["tablesCodes"].Split(',');//表编码集合
            string[] TablesName = Params["tablesNames"].Split(',');//表名称集合
            try
            {
                string SqlConnection = string.Format("server={0};database={1};uid={2};pwd={3};", ServerName, DatabaseName, ServerUser, ServerPwd);
                string exMsg = string.Empty;//异常记录
                for (int i = 0; i < TablesCode.Length; i++)//循环表编码集合
                {
                    //根据表编码，获取列字段信息（主表）
                    DataTable columnDt = DColumn.GetTableColumn(TablesName[i], SqlConnection);
                    if (columnDt.Rows.Count == 0)
                    {
                        exMsg += "主表[" + TablesName[i] + "],列字段信息为空。<br/>";
                        continue;
                    }
                    //根据表编码，获取列字段信息（副表）
                    IList<Column> columnlist = DColumn.GetColumn(new Guid(TablesCode[i]));
                    if (columnlist.Count == 0)
                    {
                        exMsg += "副表[" + TablesName[i] + "],列字段信息为空。<br/>";
                        continue;
                    }
                    //获取(副表同步主表)说明的SQL语句
                    SynchronousChief(Params, new Guid(TablesCode[i]),TablesName[i], SqlConnection, columnDt, columnlist);
                    //根据(主表同步副表)字段
                    SynchronousSide(Params, new Guid(TablesCode[i]), columnDt, columnlist);
                }
                Response.Write("{HasError:false,msg:'同步成功！<br/>" + exMsg + "'}");
            }
            catch(Exception ex)
            {
                Response.Write("{HasError:true,msg:'同步失败！<br/>异常：" + ex.Message.Replace("\r\n", "<br/>") + "'}");
            }
            Response.End();
        }
        //粘贴列字段说明信息
        public void PasteColumnBewrite() 
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string CopyTablesCode = Params["copyTablesCode"];//复制表编码
            string PasteTablesCode = Params["pasteTablesCode"];//粘贴表编码
            Response.Write(DColumn.PasteColumnBewrite(CopyTablesCode, PasteTablesCode) > 0 ? "{HasError:false,msg:'粘贴列字段说明成功！'}" : "{HasError:true,msg:'粘贴列字段说明失败！'}");
            Response.End();
        }

        /// <summary>
        /// 根据(副表同步主表)说明
        /// </summary>
        /// <param name="Params">参数</param>
        /// <param name="tablesCode">表编码</param>
        /// <param name="TablesName">表名</param>
        /// <param name="SqlConnection">链接字符串</param>
        /// <param name="columnDt">主表</param>
        /// <param name="columnlist">附表</param>
        private void SynchronousChief(NameValueCollection Params,Guid tablesCode, string TablesName, string SqlConnection, DataTable columnDt, IList<Column> columnlist)
        {
            string SynchronousSQL = string.Empty;
            for (int i = 0; i < columnlist.Count; i++)
            {
                int columnNumber = 0;//记录主表与副表的交集数
                for (int j = 0; j < columnDt.Rows.Count; j++)
                {
                    //同步主表说明的SQL
                    if (columnDt.Rows[j]["columnsName"].ToString() == columnlist[i].Name && columnlist[i].Bewrite != ""
                        && (columnDt.Rows[j]["remark"].ToString() == "" || columnDt.Rows[j]["remark"].ToString() != columnlist[i].Bewrite))
                    {
                        SynchronousSQL += string.Format("execute {4} N'MS_Description',N'{3}',N'SCHEMA',N'{0}',N'table',N'{1}',N'column',N'{2}';",
                            columnlist[i].Owner, TablesName, columnlist[i].Name, columnlist[i].Bewrite,
                            columnDt.Rows[j]["remark"].ToString() == "" ? "sp_addextendedproperty" : "sp_updateextendedproperty");
                    }
                    //同步副表说明
                    if (columnDt.Rows[j]["columnsName"].ToString() == columnlist[i].Name && columnlist[i].Bewrite == ""
                        && columnDt.Rows[j]["remark"].ToString() != "")
                    {
                        Column column = new Column();
                        column.Author = User.Identity.Name;
                        column.TablesCode = tablesCode;
                        column.Owner = columnlist[i].Owner;
                        column.Name = columnlist[i].Name;
                        column.Type = columnlist[i].Type;
                        column.Bewrite = columnDt.Rows[j]["remark"].ToString();
                        column.FieldState = 1;
                        column.HideAuthor = string.Empty;
                        column.HideTime = DateTime.Now;
                        column.Code = columnlist[i].Code;
                        DColumn.Update(column);
                    }
                    if (columnDt.Rows[j]["columnsName"].ToString() == columnlist[i].Name)
                    {
                        columnNumber++;
                    }
                }
                //附表存在作废字段
                if (columnNumber == 0)
                {
                    Column column = columnlist[i];
                    column.FieldState = 0;
                    column.HideAuthor = User.Identity.Name;
                    column.HideTime = DateTime.Now;
                    DColumn.Update(column);
                }
            }
            if (!string.IsNullOrEmpty(SynchronousSQL))
            {
                SQLHelper.ExecuteSql(SqlConnection, SynchronousSQL);
            }
        }

        /// <summary>
        /// 根据(主表同步副表)字段
        /// </summary>
        /// <param name="Params">参数</param>
        /// <param name="tablesCode">表编码</param>
        /// <param name="columnDt">主表</param>
        /// <param name="columnlist">副表</param>
        private void SynchronousSide(NameValueCollection Params, Guid tablesCode, DataTable columnDt, IList<Column> columnlist)
        {
            for (int k = 0; k < columnDt.Rows.Count; k++)
            {
                int columnNumber = 0;//记录主表与副表的交集数
                for (int q = 0; q < columnlist.Count; q++)
                {
                    if (columnDt.Rows[k]["columnsName"].ToString() == columnlist[q].Name)
                    {
                        columnNumber++;
                    }
                }
                //主表存在新的列字段
                if (columnNumber == 0)
                {
                    Column column = new Column();
                    column.Author = User.Identity.Name;
                    column.TablesCode = tablesCode;
                    column.Owner = columnDt.Rows[k]["owner"].ToString();
                    column.Name = columnDt.Rows[k]["columnsName"].ToString();
                    column.Type = columnDt.Rows[k]["columnType"].ToString();
                    column.Bewrite = columnDt.Rows[k]["remark"].ToString();
                    column.FieldState = 1;
                    column.HideAuthor = string.Empty;
                    column.HideTime = DateTime.Now;
                    column.Code = Guid.NewGuid();
                    DColumn.Add(column);
                }
            }
        }
    }
}