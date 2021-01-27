using AME.Helpers;
using AME.Services;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.InputB;
using System.Windows.Media;

namespace AME.Services
{
    public class BaseService<T1> : INotifyPropertyChanged where T1 : class, new()
    {
        public AppSetting appSetting { get; set; }

        bool isBusy = false;
        public bool IsRefreshing { get { return isBusy; } set { SetProperty(ref isBusy, value); } }

        string title = string.Empty;
        public string Title { get { return title; } set { SetProperty(ref title, value); } }
      
        string foot = string.Empty;
        public string Foot { get { return foot; } set { SetProperty(ref foot, value); } }

        int badge ;
        public int Badge { get { return badge; } set { SetProperty(ref badge, value); } }

        public DelegateCommand<string> 跳转Command { get; set; }

        //ObservableCollection WPF用,带通知,但仅仅是添加删除元素,item里面的子内容改变不会通知,要自己再写一下
        private ObservableCollection<T1> itemListOb;
        public ObservableCollection<T1> ItemListOb { get => itemListOb; set => SetProperty(ref itemListOb, value); }

        private T1 _objOneItem;
        public T1 objOneItem { get => _objOneItem; set => SetProperty(ref _objOneItem, value); }

        bool autoSearch = true;
        public bool AutoSearch { get => autoSearch; set => SetProperty(ref autoSearch, value); }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    //if (_searchText == null)return;
                    Console.WriteLine($"{Title} 输入的文字 : " + _searchText);
                }
            }
        }

        //语言
        public string CurrentLang { get; set; }
        public List<List<string>> ColLanguages = new List<List<string>>() {
            new List<string>() { "zh", "中文" },
            new List<string>() { "en", "English" },
            new List<string>() { "es", "Español" },
        };

        public BaseService()
        {
            if (ItemListOb == null)
            {
                _objOneItem = new T1();

                ItemListOb = new ObservableCollection<T1>();
                跳转Command = new DelegateCommand<string>((cmd) => ExcuteSendCommand(cmd));
            }
        }
        private void ExcuteSendCommand(string cmd)
        {
            Messenger.Default.Send<String>(SendInfo, cmd);
        }
        #region 属性
        private string sendInfo = DateTime.UtcNow.ToString();
        /// <summary>
        /// 发送消息
        /// </summary>
        public string SendInfo { get => sendInfo; set => SetProperty(ref sendInfo, value); }

        #endregion

        #region INotifyPropertyChanged
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
