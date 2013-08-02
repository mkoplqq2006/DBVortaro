using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Vortaro.Controllers.DAL;
using VortaroModel;
using System.Data;

namespace Vortaro.Controllers
{
    [Authorize]
    [HandleError]
    public class TablesController : Controller
    {
        //得到表信息
        public void GetPageTables()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string page = Params["page"];
            string rows = Params["rows"];
            int intPage = int.Parse((page == null || page == "0") ? "1" : page);
            //每页显示条数  
            int pageSize = int.Parse((rows == null || rows == "0") ? "10" : rows);
            //每页的开始记录  第一页为1  第二页为number +1   
            int start = (intPage - 1) * pageSize;
            string databaseCode = Params["databaseCode"];
            string groupCode = Params["groupCode"];
            string query = Params["query"];
            Response.Write(DTables.GetPageTables(start, pageSize, query, databaseCode, groupCode));
            Response.End();
        }
        //保存表信息
        public void SaveTables()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Tables tables = new Tables();
            tables.Author = User.Identity.Name;
            tables.DatabaseCode = new Guid(Params["databaseCode"]);
            tables.GroupCode = new Guid(Params["groupCode"]);
            tables.Name = Params["name"];
            tables.Alias = Params["alias"];
            string result = string.Empty;
            if (Params["code"] != null)
            {
                tables.Code = new Guid(Params["code"]);
                result = DTables.Update(tables) != null ? "{HasError:false,msg:'表编辑成功！'}" : "{HasError:true,msg:'表编辑失败，请稍候再试！'}";
            }
            else
            {
                tables.Code = Guid.NewGuid();
                result = DTables.Add(tables) != null ? "{HasError:false,msg:'表创建成功！'}" : "{HasError:true,msg:'表创建失败，请稍候再试！'}";
            }
            Response.Write(result);
            Response.End();
        }
        //删除表信息
        public void DeleteTables()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string[] codes = Params["codes"].Split(',');
            string result = string.Empty;
            for (int i = 0; i < codes.Length;i++ )
            {
                result = DTables.Delete(new Guid(codes[i])) != null ? "{HasError:false,msg:'表删除成功！'}" : "{HasError:true,msg:'表删除失败，请稍候再试！'}";
            }
            Response.Write(result);
            Response.End();
        }
        //得到需要导入的表
        public void GetImportTables()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string ServerName = Params["ServerName"];
            string ServerUser = Params["ServerUser"];
            string ServerPwd = Params["ServerPwd"];
            ServerPwd = NHibernateHelper.DecryptAES(ServerPwd, "zhangzhangdebing");//解密
            string DatabaseCode = Params["DatabaseCode"];//数据库编码
            string DatabaseName = Params["DatabaseName"];//数据库名称
            DataTable dt = new DataTable();
            Hashtable hasTable = new Hashtable();
            try
            {
                //根据数据库编码，得到表名称
                string tablesName = string.Empty;
                if (DatabaseCode != null)
                {
                    tablesName = DTables.GetTablesName(new Guid(DatabaseCode));
                }
                string SqlConnection = string.Format("server={0};database={1};uid={2};pwd={3};", ServerName, DatabaseName, ServerUser, ServerPwd);
                string Sql = "select name,crdate as createdate from sysobjects where xtype='U' and name!='sysdiagrams'";//排除sysdiagrams表
                if (tablesName != string.Empty)
                {
                    Sql += string.Format(" and name not in({0})",tablesName);
                }
                else 
                {
                    Sql += " order by name asc";
                }
                dt = SQLHelper.GetDataTable(SqlConnection, Sql);
                hasTable.Add("total", dt.Rows.Count);
                hasTable.Add("rows", dt);
            }
            catch(Exception ex)
            {
                NHibernateHelper.WriteErrorLog("获取需要导入的表异常", ex);
                hasTable.Add("total", 0);
                hasTable.Add("rows", dt);
                return;
            }
            Response.Write(JsonHelper.ToJson(hasTable));
            Response.End();
        }
        //导入指定表到数据库
        public void ImportTables()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            //批量添加表信息
            string[] tables = Params["tables"].Split(',');//表名称数据集
            string ServerName = Params["ServerName"];//服务器名称
            string ServerUser = Params["ServerUser"];//用户名
            string ServerPwd = Params["ServerPwd"];//密码
            ServerPwd = NHibernateHelper.DecryptAES(ServerPwd, "zhangzhangdebing");//解密
            string DatabaseName = Params["DatabaseName"];//指定数据库名称
            string databaseCode = Params["databaseCode"];//数据库信息编码
            string groupCode = Params["groupCode"];//功能分组编码
            bool resultT = false;
            try
            {
                for (int i = 0; i < tables.Length; i++)
                {
                    resultT = false;
                    string tableName = tables[i];
                    //表名是否，重复则不执行保存操作
                    if (!DTables.RepeatTablesName(tableName, databaseCode, groupCode, User.Identity.Name))
                    {
                        Tables table = new Tables();
                        table.Author = User.Identity.Name;
                        table.DatabaseCode = new Guid(databaseCode);
                        table.GroupCode = new Guid(groupCode);
                        table.Name = tableName;
                        table.Alias = tableName;
                        table.Code = Guid.NewGuid();

                        //根据表名，得到表字段信息
                        string SqlConnection = string.Format("server={0};database={1};uid={2};pwd={3};", ServerName, DatabaseName, ServerUser, ServerPwd);
                        DataTable dt = DColumn.GetTableColumn(tableName, SqlConnection);
                        if (dt.Rows.Count > 0)
                        {
                            DTables.Add(table);//添加表
                            resultT = BatchAddColumn(resultT, table, dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("导入指定表发生异常", ex);
                return;
            }
            Response.Write(resultT ? "{HasError:false,msg:'指定表导入成功！'}" : "{HasError:true,msg:'指定表导入失败，请稍候再试！'}");
            Response.End();
        }
        /// <summary>
        /// 批量添加列字段信息
        /// </summary>
        /// <param name="resultT">执行结果</param>
        /// <param name="table">表信息</param>
        /// <param name="dt">列字段信息</param>
        /// <returns></returns>
        private bool BatchAddColumn(bool resultT, Tables table, DataTable dt)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                //列名是否重复，重复则不执行保存操作
                string columnName = dt.Rows[j]["columnsName"].ToString();
                if (!DColumn.RepeatColumnName(columnName, table.Code, User.Identity.Name))
                {
                    Column column = new Column();
                    column.Author = User.Identity.Name;
                    column.TablesCode = table.Code;
                    column.Owner = dt.Rows[j]["owner"].ToString();
                    column.Name = columnName;
                    column.Type = dt.Rows[j]["columnType"].ToString();
                    column.Bewrite = dt.Rows[j]["remark"].ToString();
                    column.FieldState = 1;
                    column.HideAuthor = "";
                    column.HideTime = DateTime.Now;//默认为空
                    column.Code = Guid.NewGuid();
                    DColumn.Add(column);//添加列
                    resultT = true;
                }
            }
            return resultT;
        }
    }
}