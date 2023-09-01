using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AME.Models.Dto
{

    /// <summary>
    /// 类别
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [Table(Name = "Categories", DisableSyncStructure = true)]
    public partial class CategoriesNameOnly : DataTableBase
    {


        [JsonProperty, Column(StringLength = 50)]
        [DisplayName("类别")]
        public string CategoryName { get; set; }

        [JsonProperty, Column(StringLength = 200)]
        [DisplayName("顶级分类")]
        public string CategoryName2 { get; set; }

        [JsonProperty, Column(IsIdentity = true)]
        [DisplayName("ID")]
        public int CategoryID { get; set; }

    }
}
