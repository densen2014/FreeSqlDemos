using AME.Colors;
using AME.Helpers;
using AME.Services;
using AME.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AME.View
{
    /// <summary>
    /// ConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigPage : Window
    {

        public ConfigPage(AppSetting appSetting, ConfigViewModel dataService, PaletteSelectorViewModel paletteSelector)
        {
            InitializeComponent();
            DataContext = dataService;

            panel1.DataContext = paletteSelector;
        }

    }
}
