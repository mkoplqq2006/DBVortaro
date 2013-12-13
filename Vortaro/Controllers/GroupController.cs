using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vortaro.Controllers.DAL;
using VortaroModel;

namespace Vortaro.Controllers
{
    [Authorize]
    [HandleError]
    public class GroupController: Controller
    {
        //获取功能分组信息
        public void GetPageGroup()
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
            Response.Write(DGroup.GetPageGroup(start, pageSize, "", projectCode));
            Response.End();
        }
        //保存功能分组信息
        public void SaveGroup()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            Group group = new Group();
            group.Author = User.Identity.Name;
            group.Name = Params["name"];
            group.Bewrite = Params["bewrite"];
            group.ProjectCode = new Guid(Params["projectCode"]);
            string result = string.Empty;
            if (Params["code"] != null)
            {
                group.Code = new Guid(Params["code"]);
                result = DGroup.Update(group) != null ? "{HasError:false,msg:'功能分组编辑成功！'}" : "{HasError:true,msg:'功能分组编辑失败，请稍候再试！'}";
            }
            else
            {
                group.Code = Guid.NewGuid();
                result = DGroup.Add(group) != null ? "{HasError:false,msg:'功能分组创建成功！'}" : "{HasError:true,msg:'功能分组创建失败，请稍候再试！'}";
            }
            Response.Write(result);
            Response.End();
        }
        //删除功能分组信息
        public void DeleteGroup()
        {
            NameValueCollection Params = HttpContext.Request.Form;//参数
            string result = DGroup.Delete(new Guid(Params["code"])) != null ? "{HasError:false,msg:'功能分组删除成功！'}" : "{HasError:true,msg:'功能分组删除失败，请稍候再试！'}";
            Response.Write(result);
            Response.End();
        }
    }
}