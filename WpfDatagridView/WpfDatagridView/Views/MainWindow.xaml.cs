using AME.Colors;
using AME.Helpers;
using AME.Services;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFORMDGV;

namespace AME.View
{
    public partial class MainWindow : Window
    {

        public MainWindow(MainDataService dataService, PaletteSelectorViewModel paletteSelector, ConfigPage configPage)
        {
            InitializeComponent();

            DataContext = dataService;

            panel1.DataContext = paletteSelector;

            Messenger.Default.Register<String>(this, "打开设置", msg => configPage.ShowDialog());

            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);

            this.Closing += (sender, e) =>
            {
                Application.Current.Shutdown();//关闭
                Environment.Exit(0);
            };

        }
    }

}
