using System.Collections;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vortaro.Controllers.DAL
{
    /// <summary>
    /// 分页Json处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonHelper
    {
        /// <summary>
        /// 返回Hashtable的Json数据
        /// </summary>
        /// <param name="Hashtable">Hashtable</param>
        /// <returns></returns>
        public static string ToJson(Hashtable hasTable) 
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            return JsonConvert.SerializeObject(hasTable, Formatting.Indented, timeConverter);
        }

        /// <summary>
        /// 返回DataSet的Json数据
        /// </summary>
        /// <param name="DataSet">DataSet</param>
        /// <returns></returns>
        public static string ToJson(DataSet dataSet)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            return JsonConvert.SerializeObject(dataSet, Formatting.Indented, timeConverter);
        }
    }
}