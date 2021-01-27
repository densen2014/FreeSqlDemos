using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace csApp.Convertor
{
    public class YourConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///    <Window.Resources>
    //    <local:RowToIndexConv x:Key="RowToIndexConv"/>
    //</Window.Resources>
    //    <DataGrid ItemsSource = "{Binding GridData}" >
    //        < DataGrid.RowHeaderTemplate >
    //            < DataTemplate >
    //                < TextBlock Margin="2" Text="{Binding RelativeSource={RelativeSource AncestorType=DataGridRow}, Converter={StaticResource RowToIndexConv}}"/>
    //            </DataTemplate>
    //        </DataGrid.RowHeaderTemplate>
    //    </DataGrid>
    /// </summary>
    public class RowToIndexConv : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridRow row = value as DataGridRow;
            return row.GetIndex() + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
