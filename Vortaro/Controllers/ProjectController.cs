using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Vortaro.Controllers.DAL;
using VortaroModel;
using System.Web;

namespace Vortaro.Controllers
{
    [Authorize]
    [HandleError]
    public class ProjectController : Controller
    {
        //得到项目信息
        public void GetPageProject()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string page = Params["page"];
            string rows = Params["rows"];
            string query = Params["query"];
            int intPage = int.Parse((page == null || page == "0") ? "1" : page);
            //每页显示条数  
            int pageSize = int.Parse((rows == null || rows == "0") ? "10" : rows);
            //每页的开始记录  第一页为1  第二页为number +1   
            int start = (intPage - 1) * pageSize;
            Response.Write(DProject.GetPageProject(start, pageSize, query, User.Identity.Name));
            Response.End();
        }
        //保存项目信息
        public void SaveProject()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Project project = new Project();
            project.Author = User.Identity.Name;
            project.Name = Params["name"];
            project.Bewrite = Params["bewrite"];
            string result = string.Empty;
            if (Params["code"] != null)
            {
                project.Code = new Guid(Params["code"]);
                result = DProject.Update(project) != null ? "{HasError:false,msg:'项目编辑成功！'}" : "{HasError:true,msg:'项目编辑失败，请稍候再试！'}";
            }else{
                project.Code = Guid.NewGuid();
                result = DProject.Add(project) != null ? "{HasError:false,msg:'项目创建成功！'}" : "{HasError:true,msg:'项目创建失败，请稍候再试！'}";
            }
            Response.Write(result);
            Response.End();
        }
        //删除项目信息
        public void DeleteProject()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string result = DProject.Delete(new Guid(Params["code"])) != null ? "{HasError:false,msg:'项目删除成功！'}" : "{HasError:true,msg:'项目删除失败，请稍候再试！'}";
            Response.Write(result);
            Response.End();
        }
        //生成项目字典
        public void PublishDictionary() 
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string projectCode = Params["projectCode"];//项目编码
            string fileName = Params["projectName"];//项目名称
            string path = Server.MapPath(string.Format("~/Content/Pack/{0}", User.Identity.Name));
            string directory = Server.MapPath(string.Format("~/Content/Html/{0}/{1}", User.Identity.Name, fileName));
            try
            {
                path = GenerateHtml(projectCode, fileName, path, directory);
                do
                {
                    DZips.ZipFileDirectory(directory, path);
                } while (DZips.ValidZipFile(path));
                Response.Write("{HasError:false,msg:'项目字典生成成功！'}");
            }catch(Exception ex){
                NHibernateHelper.WriteErrorLog("生成项目字典", ex);
                Response.Write("{HasError:true,msg:'项目字典生成失败！'}");
            }
            Response.End();
        }
        //打包项目字典
        public void UnpackDictionary()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string projectCode = Params["projectCode"];//项目编码
            string fileName = Params["projectName"];//项目名称
            string path = Server.MapPath(string.Format("~/Content/Pack/{0}", User.Identity.Name));
            string directory = Server.MapPath(string.Format("~/Content/Html/{0}/{1}", User.Identity.Name, fileName));
            path = GenerateHtml(projectCode, fileName, path, directory);
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)//输出压缩包
            {
                WriteZip(fi);
            }
            else//压缩项目字典
            {
                do
                {
                    DZips.ZipFileDirectory(directory, path);
                } while (DZips.ValidZipFile(path));
                fi = new FileInfo(path);
                WriteZip(fi);
            }
        }

        #region 生成HTML
        /// <summary>
        /// 生成HTML文件，返回压缩包路径
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="fileName">项目名称</param>
        /// <param name="path">压缩包目录</param>
        /// <param name="directory">模板目录</param>
        /// <returns></returns>
        private string GenerateHtml(string projectCode, string fileName, string path, string directory)
        {
            string indexPath = Server.MapPath("~/Content/Templates/default/index.html");//首页模板路径
            string itemsPath = Server.MapPath("~/Content/Templates/default/items.html");//子页模板路径
            //判断文件夹是否存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                Directory.CreateDirectory(directory + "/Items");
                //复制文件
                string copyPath = Server.MapPath("~/Content/Templates/default/Style");
                DZips.CopyDirs(copyPath, directory + "/Style");
                System.IO.File.Copy(Server.MapPath("~/Content/Templates/default/favicon.ico"), directory + "/favicon.ico");
                //根据项目编码，生成HTML文件
                BuildIndex(projectCode, fileName, directory, indexPath, itemsPath);
            }
            else
            {
                //移除原有的HTML文件
                System.IO.File.Delete(directory + "/index.html");
                FileInfo[] files = new DirectoryInfo(directory + "\\Items").GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.Substring(files[i].Name.LastIndexOf(".") + 1) == "html")
                    {
                        System.IO.File.Delete(files[i].FullName);
                    }
                }
                //根据项目编码，生成HTML文件
                BuildIndex(projectCode, fileName, directory, indexPath, itemsPath);
            }
            path += string.Format("/{0}.zip", fileName);
            return path;
        }
        /// <summary>
        /// 生成HTML首页
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="directory">输出路径</param>
        /// <param name="indexPath">首页模板路径</param>
        /// <param name="itemsPath">子页模板路径</param>
        private static void BuildIndex(string projectCode, string fileName, string directory, string indexPath, string itemsPath)
        {
            string tableStr = string.Empty;//表格
            IList<Database> databaselist = DDatabase.GetDatabase(new Guid(projectCode));
            for (int k = 0; k < databaselist.Count; k++)
            {
                tableStr = @"<table style=""width: 100%;"">
		            <tr><td colspan=""5"" style=""font-weight:bold;line-height:20px;"">数据库：" + databaselist[k].Name + @"</td></tr>
		            <tr class=""text-center""><th style=""width:60px"">序号</th><th>表名</th><th>别名</th><th>作者</th><th>分组</th></tr>";
                IList<Tables> tableslist = DTables.GetTables(databaselist[k].Code);
                for (int q = 0; q < tableslist.Count; q++)
                {
                    string url = string.Format("Items/{0}_{1}.html", databaselist[k].Name, tableslist[q].Name);
                    tableStr += string.Format(@"<tr><td style=""padding-left:10px;"">{0}</td>
	                        <td><a href=""{1}"">{2}</a></td><td><a href=""{1}"">{3}</a></td>
	                        <td class=""text-center"">{4}</td><td class=""text-center"">{5}</td></tr>", q + 1, url, tableslist[q].Name, tableslist[q].Alias,
                        tableslist[q].Author, DGroup.GetGroupName(tableslist[q].GroupCode));
                    BuildItems(tableslist[q].Code, tableslist[q].Name, tableslist[q].Alias, directory + "/" + url, itemsPath);
                }
                tableStr += "</table><br />";
            }
            string[] indexTemp = new string[] { "@title", "@table", "@footer" };
            string[] indexHtml = new string[] { fileName, tableStr, "作者：张德兵" };
            DTemplates.WriteHtml(indexTemp, indexHtml, indexPath, directory + "/index.html");
        }
        /// <summary>
        /// 生成HTML子页
        /// </summary>
        /// <param name="tableCode">表编码</param>
        /// <param name="tableName">表名称</param>
        /// <param name="tableAlias">表别名</param>
        /// <param name="directory">输出路径</param>
        /// <param name="itemsPath">模板路径</param>
        private static void BuildItems(Guid tableCode, string tableName, string tableAlias, string directory, string itemsPath)
        {
           
            IList<Column> columnlist = DColumn.GetColumn(tableCode);
            string tableStr = @"<table style=""width: 100%;"">
            <tr class=""text-center""><th style=""width:60px"">序号</th><th>字段</th><th>类型</th><th>说明</th><th>作者</th><th>时间</th></tr>";
            for (int k = 0; k < columnlist.Count; k++)
            {
                tableStr += string.Format(@"<tr><td style=""padding-left:10px;"">{0}</td>
                <td>{1}</td><td>{2}</td><td>{3}</td><td class=""text-center"">{4}</td><td class=""text-center"">{5}</td></tr>",
                k+1,columnlist[k].Name,columnlist[k].Type,columnlist[k].Bewrite,columnlist[k].Author,columnlist[k].CreateTime);
            }
            tableStr += "</table>";
            string[] itemsTemp = new string[] { "@title", "@name", "@alias", "@table", "@footer" };
            string[] itemsHtml = new string[] { tableAlias, tableName, tableAlias, tableStr, "作者：张德兵" };
            DTemplates.WriteHtml(itemsTemp, itemsHtml, itemsPath, directory);
        }
        #endregion

        /// <summary>
        /// 输出压缩包
        /// </summary>
        /// <param name="fi">文件</param>
        private void WriteZip(FileInfo fi)
        {
            string outputFileName = string.Empty,browser = Request.UserAgent.ToUpper();
            if (browser.Contains("MS") == true && browser.Contains("IE") == true)
            {
                outputFileName = Server.UrlEncode(fi.Name);
            }
            else if (browser.Contains("FIREFOX") == true)
            {
                outputFileName = fi.Name;
            }
            else
            {
                outputFileName = Server.UrlEncode(fi.Name);
            }
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + outputFileName);
            Response.AddHeader("Content-Length", fi.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.Filter.Close();
            Response.WriteFile(fi.FullName);
            Response.End();
        }
        //根据项目编码，获取数据库以及表信息(用于主页展示目录)
        public void PreviewTable() 
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            IList<Database> databaselist = DDatabase.GetDatabase(new Guid(Params["projectCode"].ToString()));
            StringBuilder Json = new StringBuilder();
            Json.Append("{ projectName: '" + Params["projectName"] + "',");
            Json.Append("copyright:'张德兵',");
            Json.Append("time: '" + DateTime.Now.ToString("yyyy年MM月dd日") + "',list: [");
            for (int i = 0; i < databaselist.Count;i++ )
            {
                if (i > 0) { Json.Append(","); }
                Json.Append("{ databaseName: '" + databaselist[i].Name + "',list: [");
                IList<Tables> tableslist = DTables.GetTables(databaselist[i].Code);
                for (int j = 0; j < tableslist.Count; j++)
                {
                    if (j > 0) { Json.Append(","); }
                    Json.Append("{ Name: '"+tableslist[j].Name+"',");
                    Json.Append("Url: 'Items/items.html?code="+tableslist[j].Code+"',");
                    Json.Append("Alias: '" + tableslist[j].Alias + "',");
                    Json.Append("Author: '" + tableslist[j].Author + "',");
                    Json.Append("GroupName: '" + DGroup.GetGroupName(tableslist[j].GroupCode) + "'");
                    Json.Append("}");
                }
                Json.Append("]}");
            }
            Json.Append("]}");
            Response.Write(Json.ToString());
            Response.End();
        }
        //根据表编码，获取列名字段信息(用于子页展示字段信息)
        public void PreviewColumn()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Guid tableCode = new Guid(Params["tableCode"]);
            IList<Column> columnlist = DColumn.GetColumn(tableCode);
            //根据表编码得到表信息
            Tables tables = DTables.GetProjectById(tableCode);
            StringBuilder Json = new StringBuilder();
            Json.Append("{ Name: '" + tables.Name + "',");//表名
            Json.Append("Alias:'"+ tables.Alias +"',");//别名
            Json.Append("copyright:'张德兵',");
            Json.Append("list: [");
            for (int i = 0; i < columnlist.Count; i++)
            {
                if (i > 0) { Json.Append(","); }
                Json.Append("{ Name: '" + columnlist[i].Name + "',");
                Json.Append("Type: '" + columnlist[i].Type + "',");
                Json.Append("Bewrite: '" + columnlist[i].Bewrite + "',");
                Json.Append("Author: '" + columnlist[i].Author + "',");
                Json.Append("CreateTime: '" + columnlist[i].CreateTime + "'");
                Json.Append("}");
            }
            Json.Append("]}");
            Response.Write(Json.ToString().Replace("\'", "\""));
            Response.End();
        }
    }
}