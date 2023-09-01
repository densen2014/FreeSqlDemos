using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AME.Models.Dto;
using AME.Models.Entity;
 
namespace AME.Models.Restaurant;


/// <summary>
/// 商品
/// </summary>
[Index("uk_BarCode", "BarCode", true)]
[Index("uk_UserCode", "UserCode", true)]
[JsonObject(MemberSerialization.OptIn)]
[Table(Name = "Products", DisableSyncStructure = true)]
public partial class ResProducts : DataTableBase
{

    /// <summary>
    /// 商品标识
    /// </summary>
    [DisplayName("ID")]
    [JsonProperty, Column(IsIdentity = true, Position = 0)]
    public int ProductID { get; set; }

    /// <summary>
    /// 编号
    /// </summary>
    [JsonProperty, Column(StringLength = 128, Position = 1)]
    [DisplayName("编号")]
    public string UserCode { get => userCode; set => SetProperty(ref userCode, value); }
    private string userCode;

    /// <summary>
    /// 条码
    /// </summary>
    [JsonProperty, Column(StringLength = 128, Position = 2)]
    [DisplayName("条码")]
    public string BarCode { get => barCode; set => SetProperty(ref barCode, value); }
    private string barCode;

    /// <summary>
    /// 商品名称
    /// </summary>
    [JsonProperty, Column(Position = 3)]
    [DisplayName("名称")]
    public string ProductName { get => productName; set => SetProperty(ref productName, value); }
    private string productName;

    /// <summary>
    /// 售价
    /// </summary>
    [JsonProperty, Column(DbType = "decimal(19,4)", Position = 7)]
    [DisplayName("售价")]
    public decimal UnitPrice { get => unitPrice; set => SetProperty(ref unitPrice, value); }
    private decimal unitPrice;

    [JsonProperty, Column(DbType = "decimal(19,4)", Position = 8)]
    [DisplayName("售价2")]
    public decimal? UnitPrice2 { get; set; } = 0;

    /// <summary>
    /// 税率
    /// </summary>
    [JsonProperty, Column(Position = 15)]
    [DisplayName("税率")]
    public float Tax { get => tax; set => SetProperty(ref tax, value); }
    private float tax = 0.21f;

    /// <summary>
    /// 折扣
    /// </summary>
    [JsonProperty, Column(Position = 16)]
    [DisplayName("折扣")]
    public int Discount { get => discount; set => SetProperty(ref discount, value); }
    private int discount = 0;

    [JsonProperty]
    [DisplayName("名称2")]
    public string ProductName2 { get => productName2; set => SetProperty(ref productName2, value); }
    private string productName2;

    /// <summary>
    /// 类别标识
    /// </summary>
    [JsonProperty]
    public int? CategoryID { get; set; } = 0;

    [JsonProperty, Column(StringLength = -1)]
    [DisplayName("备注")]
    public string Remark { get; set; }

    [JsonProperty, Column(StringLength = -1)]
    public string Picture { get; set; }

    [Column(IsIgnore = true)]
    public string CategoryName { get => categoryName??resCategories?.CategoryName ?? ""; set => categoryName = value; }
    string categoryName;

    /// <summary>
    /// [餐馆] 触摸按钮标签颜色
    /// </summary>
    [JsonProperty, Column(StringLength = 128)]
    [DisplayName("颜色")]
    public string Color { get; set; }

    /// <summary>
    /// [餐馆] 今日限售数量
    /// </summary>
    [JsonProperty]
    public int? limitQty { get; set; }

    /// <summary>
    /// [餐馆] 父级
    /// </summary>
    [JsonProperty, Column(StringLength = 128)]
    public string Parent { get; set; }

    /// <summary>
    /// 类别
    /// </summary>
    [JsonProperty, Navigate(nameof(CategoryID))]
    public virtual ResCategories resCategories { get; set; }

    /// <summary>
    /// 包装(每包12)
    /// </summary>
    [JsonProperty]
    public int QuantityPerUnit { get; set; } = 1;

    /// <summary>
    /// 大包装(每箱144)
    /// </summary>
    [JsonProperty]
    public int QuantityPerUnit2nd { get; set; } = 1;

    [JsonProperty]
    public double? UnitsOnOrder { get; set; } = 0;

    /// <summary>
    /// 指定打印通道
    /// </summary>
    [DisplayName("指定打印通道")]
    [JsonProperty]
    public int? Option2 { get; set; }

}


[Table(Name = "Products", DisableSyncStructure = true)]
public partial class ProductsRes1 : Products
{
    #region 外键 => 导航属性，OneToMany

    [Navigate(nameof(ProductID))]
    public virtual List<ResOrderDetails> OrderDetailss { get; set; }


    #endregion

}
