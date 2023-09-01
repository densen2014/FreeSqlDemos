using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AME.Models.Dto;
 
namespace AME.Models.Restaurant
{

    [Index("uk_CustomerID", "CustomerID", true)]
    [JsonObject(MemberSerialization.OptIn), Table(Name = "Customers")]
    public partial class ResCustomersLite : DataTableBase
    {
        [JsonProperty, Column(IsPrimary = true)]
        public string CustomerID { get; set; }

        [JsonProperty]
        public string CompanyName { get; set; }

        [JsonProperty]
        public string Phone { get; set; }

        [JsonProperty]
        public string Address { get; set; }

        [JsonProperty]
        public string City { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        [JsonProperty, Column(StringLength = 50)]
        [DisplayName("地区")]
        public string Region { get; set; }

        [JsonProperty]
        public string PostalCode { get; set; }

        [JsonProperty]
        public string TaxNumber { get; set; }

        [JsonProperty]
        public int? CusGroup { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        public string Color { get; set; }

        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("分组")]
        public string CusGroupname { get => cusGroupname ?? $"{groupName?.CusGroupname ?? CusGroup.ToString()}"; set => cusGroupname = value; }
        string cusGroupname;


        [JsonProperty, Column(IsIgnore = true)]
        [DisplayName("分组价格")]
        public bool UsePrice2 { get => usePrice2 ?? (groupName?.UsePrice2 ?? false); set => usePrice2 = value; }
        bool? usePrice2;

        [JsonProperty]
        public string username { get; set; }

        [JsonProperty]
        public string password { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        public string PaymentType { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        public string ShipType { get; set; }

        [JsonProperty, Column(StringLength = 128)]
        public string wechat { get; set; }

        #region 外键 => 导航属性，OneToMany

        [Navigate(nameof(CustomerID))]
        public virtual List<ResOrders> Orderss { get; set; }

        #endregion

        #region 外键 => 导航属性，ManyToMany
        [Navigate(nameof(CusGroup))]
        //[Navigate("CusGroup")]
        public virtual CusGroupName groupName { get; set; }

        #endregion

 
    }

    
}

namespace AME.Models.Dto
{
    public partial class DataTableBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// 通知并保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingStore"></param>
        /// <param name="value"></param>
        /// <param name="onChanged"></param>
        /// <param name="propertyName"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T backingStore, T value, Action onChanged, [CallerMemberName] string propertyName = "", bool force = false) =>
                 SetProperty(ref backingStore, value, propertyName, onChanged, force);

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null, bool force = false)
        {
            if (!force && EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }


}
