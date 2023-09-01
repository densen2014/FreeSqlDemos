using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AME.Models.Dto;
using AME.Models.Entity;

namespace AME.Models.Restaurant
{

    [JsonObject(MemberSerialization.OptIn), Table(Name = "Order Details")]
    public partial class ResOrderDetails : DataTableBase
    {

        [JsonProperty, Column(IsIdentity = true)]
        public int ID { get; set; }

        [JsonProperty]
        public int? OrderID
        {
            get => _OrderID; set
            {
                if (_OrderID == value) return;
                _OrderID = value;
                Orders = null;
            }
        }
        private int? _OrderID;

        [JsonProperty]
        public int ProductID { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        [DisplayName("编号")]
        public string UserCode { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        [DisplayName("菜品")]
        public string ProductName { get; set; }

        [JsonProperty, Column(DbType = "numeric(18,2)")]
        [DisplayName("数量")]
        public decimal? Quantity { get => quantity; set { if (SetProperty(ref quantity, value)) { OnPropertyChanged("QuantityInt"); OnPropertyChanged("QuantityExist"); } } }
        decimal? quantity;

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("数量")]
        public string QuantityInt { get => (Quantity == null || Quantity == 0) ? "" : Convert.ToInt32(Quantity).ToString("N0"); }

        [JsonProperty, Column(DbType = "decimal(19,4)")]
        [DisplayName("单价")]
        public decimal UnitPrice { get => unitPrice; set => SetProperty(ref unitPrice, value); }
        decimal unitPrice;

        [JsonProperty, Column(StringLength = 300)]
        public string Action { get; set; }

        [JsonProperty]
        [DisplayName("折扣")]
        public decimal? Discount { get => _Discount??0; set => SetProperty(ref _Discount, value??0); }
        decimal? _Discount = 0;

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("数量有效")]
        public bool QuantityExist { get => !(Quantity == null || Quantity == 0); }

        [JsonProperty, Column(DbType = "numeric(18,2)")]
        [DisplayName("厨单数量")]
        public decimal? PrnQty { get => prnQty; set => SetProperty(ref prnQty, value); }
        decimal? prnQty;


        [JsonProperty, Column(StringLength = 300)]
        [DisplayName("备注")]
        public string Remark { get; set; }

        [JsonProperty]
        public int? tag { get; set; }

        [JsonProperty]
        [DisplayName("税%")]
        public float? Tax { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        public bool Synced { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        public string PIC { get { return pic; } set { SetProperty(ref pic, value, force: true); } }
        string pic;

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("税额")]
        public decimal TaxTotal
        {
            get
            {
                var v = Convert.ToDecimal((Tax ?? 0.1d) / (1d + Tax ?? 0.1d));
                return (Quantity ?? 0) * (UnitPrice * v) * (100 - Convert.ToDecimal(Discount)) / 100;
            }
            set => SetProperty(ref _TaxTotal, value);
        }
        private decimal _TaxTotal;

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("不含税")]
        public decimal BaseTotal
        {
            get
            {
                var v = Convert.ToDecimal(1 / (1d + Tax ?? 0.1d));
                return (Quantity ?? 0) * (UnitPrice * v) * (100 - Convert.ToDecimal(Discount)) / 100;
            }
            set => SetProperty(ref _BaseTotal, value);
        }
        private decimal _BaseTotal;

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("合计")]
        public decimal Total
        {
            get => (Quantity ?? 0) * UnitPrice * (100 - Convert.ToDecimal(Discount)) / 100;
            set => SetProperty(ref _Total, value);
        }
        private decimal _Total;

        /// <summary>
        /// 图片
        /// </summary>
        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("图片")]
        public string PhotoUrl { get => $"Product/{UserCode}.jpg"; set => _ = value; }

        [JsonProperty, Column(DbType = "decimal(19,4)")]
        [DisplayName("不含税")]
        public decimal? PriceBase { get; set; }

        #region 外键 => 导航属性，ManyToOne/OneToOne

        [Navigate(nameof(OrderID))]
        public virtual ResOrders Orders { get; set; }

        [Navigate(nameof(ProductID))]
        public virtual Products Products { get; set; }

        [Navigate(nameof(ProductID))]
        public virtual ResProducts ProductsLite { get; set; }

        [Navigate(nameof(ProductID))]
        public virtual ResCategories Category { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("打印分组")]
        public int? PrintGroup { get => printGroup ?? Category?.PrintGroup ?? 0; set => printGroup = value; }
        int? printGroup;

        [Navigate(nameof(ProductID))]
        public virtual CategoriesNameOnly ProductsDtoName2s { get; set; }

        #endregion 



    }

}
