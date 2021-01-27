using AME.Helpers;
using AME.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.InputB;
using WPFORMDGV;

namespace AME.ViewModels
{
    public class ConfigViewModel : BaseService<AppSetting>
    {
        public string ipaddress { get; set; }

        public ConfigViewModel(AppSetting appSetting)
        {
            this.appSetting = appSetting;
            Title = "配置";
        }



    }
}