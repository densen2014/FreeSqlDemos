using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AME.Services
{
    public static class Constants
    {
    }


    public class MyService : INotifyPropertyChanged
    {
 
        public MyService()
        {
         }

 
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

    public class FileObject
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public double Bytes { get; set; }
        public DateTime Lastmodify { get; set; }
        public string HashCode { get; set; }
        public string LocalHashCode { get; set; }
        public string Result { get; set; }

    }
     
}
