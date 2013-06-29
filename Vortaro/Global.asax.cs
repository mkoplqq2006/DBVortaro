using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Vortaro.Controllers.DAL;

namespace Vortaro
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server.GetLastError() == null)
            {
                return;
            }
            Exception ex = Server.GetLastError().GetBaseException();
            StringBuilder sb = new StringBuilder();
            sb.Append(ex.Message);
            sb.Append("\r\n来源: " + ex.Source);
            if (Request.Form != null)
            {
                sb.Append("\r\nFORM请求: " + this.Request.Form.ToString());
            }
            if (Request.QueryString != null)
            {
                sb.Append("\r\n查询字符串: " + this.Request.QueryString.ToString());
            }
            sb.Append("\r\n引发当前异常的原因: " + ex.TargetSite);
            sb.Append("\r\n堆栈跟踪: " + ex.StackTrace);
            NHibernateHelper.WriteErrorLog(sb.ToString());
            var key = System.Configuration.ConfigurationManager.AppSettings["IsDebug"];
            bool isDebug;
            if (!bool.TryParse(key, out isDebug) || !isDebug)
            {
                this.Server.ClearError();
            }
        }
    }
}