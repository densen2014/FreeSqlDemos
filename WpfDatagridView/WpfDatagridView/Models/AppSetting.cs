using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AME.Helpers
{
    public class AppSetting : INotifyPropertyChanged
    {
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

        #region 单例
        private static string FileName = Process.GetCurrentProcess().MainModule.ModuleName.ToString() + ".ini";
        private static AppSetting instance;
        public static AppSetting Create(string filename, bool force = false)
        {
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + $"-----  调设置 {filename} -----  ");
            //单例
            if (instance == null || force)
            {
                FileName = filename;
                Debug.WriteLine(DateTime.Now.ToLongTimeString() + "-----  初始化本地设置 -----  ");
                try
                {
                    if (System.IO.File.Exists(FileName))
                    {
                        string TestFileStream = File.ReadAllText(FileName);
                        try
                        {
                            if (string.IsNullOrWhiteSpace(TestFileStream) == false) instance = JsonConvert.DeserializeObject<AppSetting>(TestFileStream);//反序列化
                        }
                        finally
                        {
                        }
                    }
                }
                finally
                {
                }
            }
            if (instance == null)
            {
                instance = new AppSetting();
            }
            if (force && instance == null) instance.FilePath = filename;
            return instance;
        }

        static bool saveBusy;
        public async static void SaveSettings()
        {
            //string jsonData = JsonConvert.SerializeObject(instance);
            //File.WriteAllText(FileName, jsonData);
            if (saveBusy) return; saveBusy = true;
            try
            {
                await Task.Run(() =>
                {
                    Debug.WriteLine(DateTime.Now.ToLongTimeString() + $"-----  保存设置 {FileName} -----  ");
                    //Json序列化
                    string jsonData = JsonConvert.SerializeObject(instance);
                    File.WriteAllText(instance.FilePath ?? FileName, jsonData);
                });

            }
            finally
            {
                saveBusy = false;
            }

        }

        //app使用
        public async void SaveSettingss()
        {
            //string jsonData = JsonConvert.SerializeObject(instance);
            //File.WriteAllText(FileName, jsonData);
            if (saveBusy) return; saveBusy = true;
            try
            {
                await Task.Run(() =>
                {
                    //Json序列化
                    string jsonData = JsonConvert.SerializeObject(instance);
                    File.WriteAllText(FileName, jsonData);
                });

            }
            finally
            {
                saveBusy = false;
            }

        }
        #endregion


        public AppSetting()
        {
        }
        public void SaveAI()
        {
            if (instance != null) SaveSettings();
        }

        [JsonIgnore]
        public string FilePath { get; set; }

        private bool _isdebug;
        public bool isdebug { get => _isdebug; set { if (SetProperty(ref _isdebug, value)) SaveAI(); } }

        bool isDark = false;
        public bool IsDark { get => isDark; set { if (SetProperty(ref isDark, value)) SaveAI(); } }

        string mainWindowMax = "Normal";
        public string MainWindowMax { get => mainWindowMax; set { if (SetProperty(ref mainWindowMax, value)) SaveAI(); } }

        string myFontFamily = "幼圆";
        public string MyFontFamily { get => myFontFamily; set { if (SetProperty(ref myFontFamily, value)) SaveAI(); } }

        int myfontsize = 13;
        public int Myfontsize { get => myfontsize; set { if (SetProperty(ref myfontsize, value)) SaveAI(); } }

    }


}
