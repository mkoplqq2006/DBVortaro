using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using Vortaro.Controllers.DAL;
using VortaroModel;

namespace Vortaro.Controllers
{
    [Authorize]
    [HandleError]
    public class DatabaseController : Controller
    {
        //得到数据库信息
        public void GetPageDatabase()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string page = Params["page"];
            string rows = Params["rows"];
            int intPage = int.Parse((page == null || page == "0") ? "1" : page);
            //每页显示条数  
            int pageSize = int.Parse((rows == null || rows == "0") ? "10" : rows);
            //每页的开始记录  第一页为1  第二页为number +1   
            int start = (intPage - 1) * pageSize;
            string projectCode = Params["projectCode"];
            Response.Write(DDatabase.GetPageDatabase(start, pageSize, "", projectCode));
            Response.End();
        }
        //保存数据库信息
        public void SaveDatabase()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Database database = new Database();
            database.Author = User.Identity.Name;
            database.ProjectCode = new Guid(Params["projectCode"]);
            database.Name = Params["name"];
            database.Alias = Params["alias"];
            database.Type = Params["type"];
            database.ServerName = Params["serverName"];
            database.ServerUser = Params["serverUser"];
            database.ServerPwd = NHibernateHelper.EncryptAES(Params["serverPwd"], "zhangzhangdebing");//加密
            database.Bewrite = Params["bewrite"];
            string result = string.Empty;
            if (Params["code"] != null)
            {
                database.Code = new Guid(Params["code"]);
                result = DDatabase.Update(database) != null ? "{HasError:false,msg:'数据库编辑成功！'}" : "{HasError:true,msg:'数据库编辑失败，请稍候再试！'}";
            }
            else
            {
                database.Code = Guid.NewGuid();
                result = DDatabase.Add(database) != null ? "{HasError:false,msg:'数据库创建成功！'}" : "{HasError:true,msg:'数据库创建失败，请稍候再试！'}";
            }
            Response.Write(result);
            Response.End();
        }
        //删除数据库信息
        public void DeleteDatabase()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string result = DDatabase.Delete(new Guid(Params["code"])) != null ? "{HasError:false,msg:'数据库删除成功！'}" : "{HasError:true,msg:'数据库删除失败，请稍候再试！'}";
            Response.Write(result);
            Response.End();
        }
        //连接数据库
        public void ConnectionDatabase()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Database database = new Database();
            database.Name = Params["name"];
            database.ServerName = Params["serverName"];
            database.ServerUser = Params["serverUser"];
            database.ServerPwd = Params["serverPwd"];//未加密
            string result = DDatabase.ConnectionDatabase(database) ? "{HasError:false,msg:'连接成功！'}" : "{HasError:true,msg:'连接失败！'}";
            Response.Write(result);
            Response.End();
        }
    }
}