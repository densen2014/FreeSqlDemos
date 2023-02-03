using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Threading;
using System.Data.Common;

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
}
