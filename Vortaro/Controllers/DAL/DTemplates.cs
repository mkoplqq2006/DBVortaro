using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 模板操作类
    /// </summary>
    public class DTemplates
    {
        /// <summary>
        /// 读取模板,生成静态HTML
        /// </summary>
        /// <param name="TempElement">模板标签</param>
        /// <param name="HtmlElement">Html元素</param>
        /// <param name="ReadFile">读取文件路径</param>
        /// <param name="OutFile">输出文件路径</param>
        /// <returns></returns>
        public static bool WriteHtml(string[] TempElement, string[] HtmlElement, string ReadFile, string OutFile)
        {
            bool result = false;//生成结果
            Encoding code = Encoding.GetEncoding("utf-8");
            StreamReader sr = null;
            StreamWriter sw = null;
            string str = "";
            try
            {
                //读取模板内容
                sr = new StreamReader(ReadFile, code);
                str = sr.ReadToEnd(); 
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("读取模板内容", ex);
                if (sr != null)
                {
                    sr.Close();
                }
            }
            //替换内容
            for (int i = 0; i < TempElement.Length; i++)
            {
                str = str.Replace(TempElement[i], HtmlElement[i]);
            }
            try
            {
                sw = new StreamWriter(OutFile, false, code);
                sw.Write(str);
                sw.Flush();
                result = true;
            }
            catch (Exception ex)
            {
                NHibernateHelper.WriteErrorLog("读取模板,生成静态HTML", ex);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
            return result;
        }

    }
}