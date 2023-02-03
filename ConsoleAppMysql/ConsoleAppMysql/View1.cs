using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Threading;
using System.Data.Common;
using System.Reflection;

namespace MyTestDB
{

    /// <summary>
    /// VIEW
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
    public partial class View1
    {

        [JsonProperty]
        public int id { get; set; } = 0;

        [JsonProperty]
        public string remark { get; set; }

    }

    [ExpressionCall]
    public static class DbFunc
    {
        //必要定义 static + ThreadLocal
        static ThreadLocal<ExpressionCallContext> context = new ThreadLocal<ExpressionCallContext>();

        public static DbParameter DefaultValue(this DbParameter that, string parameter)
        {
            that.ParameterName = parameter;
            that.Value = Guid.NewGuid().ToString();
            return that;
        }
    }

    public static class DbFunc2
    {
        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <returns></returns>
        public static Dictionary<string, object> EntityToMap(this object obj) => JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjectToMap(this object obj)
        {
            Dictionary<string, string> map = new Dictionary<string, string>(); //

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型string

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性io

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();
                if (m != null && m.IsPublic)
                {
                    // 进行判NULL处理
                    if (m.Invoke(obj, new object[] { }) != null)
                    {
                        map.Add(p.Name, m.Invoke(obj, new object[] { }).ToString()); // 向字典添加元素
                    }
                }
            }
            return map;
        }

    }
}
