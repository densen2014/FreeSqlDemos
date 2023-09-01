using AME.Models.Dto;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AME.Models.Restaurant
{

    [JsonObject(MemberSerialization.OptIn), Table(Name = "Categories")]
    public partial class ResCategories : DataTableBase
    {

        [JsonProperty, Column(IsIdentity = true)]
        public int CategoryID { get; set; }

        [JsonProperty]
        public string CategoryName { get; set; }

        [JsonProperty, Column(StringLength =-1)] // 原ntext
        public string Description { get; set; }

        [JsonProperty]//, Column(DbType = "image")]//图片字段,mssql和sqlite支持,mysql不支持
        public byte[] Picture { get; set; }

        [JsonProperty]
        public bool? PrintCookTicket { get; set; }

        [JsonProperty]
        public bool? PrintCookTicket2 { get; set; }

        [JsonProperty]
        public bool? PrintCookTicketII { get; set; }

        [JsonProperty]
        public int? PrintGroup { get; set; }


        #region 外键 => 导航属性，ManyToMany

        #endregion
    }


    /// <summary>
    /// 类别
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "Categories")]
    public class ResCategoriesDto
    {
        /// <summary>
        /// 类别标识
        /// </summary>
        [JsonProperty, Column(IsIdentity = true)]
        public int CategoryID { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        [JsonProperty]
        public string CategoryName { get; set; }
    }


    /// <summary>
    /// 下单类别中间表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(Name = "Categories",DisableSyncStructure =true)]
    public partial class ResCategoriesOrders : DataTableBase
    {

        [JsonProperty, Column(IsIdentity = true)]
        public int CategoryID { get; set; }

        [JsonProperty]
        public string CategoryName { get; set; }

        [JsonProperty]
        public bool? PrintCookTicket { get; set; }

        [JsonProperty]
        public bool? PrintCookTicket2 { get; set; }

        [JsonProperty]
        public bool? PrintCookTicketII { get; set; }

        [JsonProperty]
        public int? PrintGroup { get; set; }

        [JsonProperty, Column(IsIgnore =true)]
        [DisplayName("数量")]
        public int? Quantity { get => quantity; set => SetProperty(ref quantity, value); }
        int? quantity;
 
 
    }

}
