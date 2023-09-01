using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;
using AME.Models.Dto;

namespace AME.Models.Restaurant
{

    [JsonObject(MemberSerialization.OptIn), Table(Name = "Orders")]
    public partial class ResOrdersLite : DataTableBase
    {
        /// <summary>
        /// 流水号
        /// </summary>
        [JsonProperty, Column(IsPrimary = true)]
        [DisplayName("流水号")]
        public int OrderID { get; set; }

        /// <summary>
        /// 单据日期
        /// </summary>
        [JsonProperty]
        [DisplayName("日期")]
        public DateTime? OrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 台号/客户
        /// </summary>
        [DisplayName("台号/客户")]
        [JsonProperty]
        public string CustomerID
        {
            get => _CustomerID; set
            {
                if (_CustomerID == value) return;
                _CustomerID = value;
                Customers = null;
            }
        }
        private string _CustomerID;

        /// <summary>
        /// 订单
        /// </summary>
        [DisplayName("订单")]
        [JsonProperty]
        public int? AlbaranID { get; set; }

        /// <summary>
        /// 发票
        /// </summary>
        [DisplayName("发票")]
        [JsonProperty]
        public int? FacturaID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty, Column(StringLength = 50)]
        [DisplayName("状态")]
        public string Status { get; set; }


        /// <summary>
        /// 合计金额
        /// </summary>
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        [DisplayName("合计")]
        public decimal? SubTotal { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [DisplayName("付款方式")]
        [JsonProperty, Column(StringLength = 50)]
        public string PayMode { get; set; }

        /// <summary>
        /// 现金
        /// </summary>
        [DisplayName("现金")]
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? Cash { get; set; }

        /// <summary>
        /// VISA
        /// </summary>
        [DisplayName("VISA")]
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? VISA { get; set; }

        /// <summary>
        /// 代金券
        /// </summary>
        [DisplayName("代金券")]
        public decimal? Vouchers { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        [DisplayName("其他")]
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? PayOthers { get; set; }

        [JsonProperty]
        [DisplayName("折扣合计")]
        public float? Discount { get; set; }

        /// <summary>
        /// 历史金额
        /// </summary>
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        [DisplayName("历史金额")]
        public decimal? HisTotal { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [JsonProperty, Column(StringLength = 100)]
        [DisplayName("发票抬头")]
        public string InvoiceLabel { get; set; }

        /// <summary>
        /// 电子发票UUID
        /// </summary>
        [JsonProperty, Column(StringLength = 450)]
        [DisplayName("电子发票UUID")]
        public string EInvoiceUUID { get; set; }

        /// <summary>
        /// 电子发票Note
        /// </summary>
        [JsonProperty, Column(StringLength = 450)]
        [DisplayName("电子发票Note")]
        public string EInvoiceNote { get; set; }

        /// <summary>
        /// 电子发票号码
        /// </summary>
        [JsonProperty, Column(StringLength = 100)]
        [DisplayName("电子发票号码")]
        public string EInvoiceNumber { get; set; }

        #region 外键 => 导航属性，ManyToOne/OneToOne

        [Navigate(nameof(CustomerID))]
        public virtual ResCustomersLite Customers { get; set; }

        #endregion

        #region 外键 => 导航属性，OneToMany

        [Navigate(nameof(OrderID))]
        public virtual List<ResOrderDetails> orderDetailss { get; set; }

        #endregion

    }

    [JsonObject(MemberSerialization.OptIn), Table(Name = "Orders")]
    public partial class ResOrders: ResOrdersLite
    {


        /// <summary>
        /// 员工
        /// </summary>
        [JsonProperty, Column(StringLength = 50)]
        [DisplayName("员工")]
        public string Employe { get; set; }

        /// <summary>
        /// 净金额
        /// </summary>
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        [DisplayName("净金额")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        [JsonProperty]
        [DisplayName("人数")]
        public int? Body { get; set; }

        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? AccountReceivable { get; set; }

        [JsonProperty]
        public DateTime? CloseDate { get; set; }

        [JsonProperty]
        public DateTime? CloseTime { get; set; }

        [JsonProperty, Column(StringLength = 50)]
        public string Del { get; set; }

        [JsonProperty]
        public int? EmployeeID { get; set; }

        [JsonProperty, Column(StringLength = 50)]
        public string Final { get; set; }

        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? Freight { get; set; }

        [JsonProperty]
        public DateTime? LimitDate { get; set; }

        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? MakeVoucher { get; set; }

        [JsonProperty]
        [DisplayName("单据时间")]
        public DateTime? OrderTime { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        public string PaymentType { get; set; }

        [JsonProperty, Column(StringLength = 800)]
        public string PrintedNoChgOrdersDetails { get; set; }

        [DisplayName("备注")]
        [JsonProperty, Column(StringLength = -1)]
        public string Remark { get; set; } 

        /// <summary>
        /// 已收金额
        /// </summary>
        [DisplayName("已收金额")]
        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? Receivd { get; set; }

        /// <summary>
        /// 单据时间
        /// </summary>
        [JsonProperty]
        [DisplayName("单据时间")]
        public DateTime? RequiredDate { get; set; }= DateTime.Now.Date;

        [JsonProperty, Column(StringLength = 60)]
        public string ShipAddress { get; set; }

        [JsonProperty]
        public string ShipCity { get; set; }

        [JsonProperty, Column(StringLength = 15)]
        public string ShipCountry { get; set; }

        /// <summary>
        /// ShipCustomerID = "0" 普通客户
        /// </summary>
        [JsonProperty]
        public string ShipCustomerID { get; set; }

        [JsonProperty, Column(StringLength = 40)]
        public string ShipName { get; set; }

        [JsonProperty]
        [DisplayName("送餐时间")]
        public DateTime? ShippedDate { get; set; }

        [JsonProperty, Column(StringLength = 10)]
        public string ShipPostalCode { get; set; }

        [JsonProperty, Column(StringLength = 15)]
        public string ShipRegion { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        public string ShipType { get; set; }

        [JsonProperty]
        public int? ShipVia { get; set; }

        [JsonProperty]
        public bool TaxInclude { get; set; }

        [JsonProperty, Column(StringLength = 500)]
        public string ticketInfo { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        [JsonProperty, Column(StringLength = 150)]
        [DisplayName("发票抬头")]
        public string txtInvoice { get; set; }

        [JsonProperty, Column(DbType = "decimal(19,4)")]
        public decimal? ValueAddTax { get; set; }

        [JsonProperty, Column(StringLength = 50)]
        public string VouchersID { get; set; }


 
    }

}
