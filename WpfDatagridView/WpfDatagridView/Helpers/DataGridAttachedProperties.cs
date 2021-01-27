using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AME.Triggers
{
    /// <summary>
    /////                     <DataGrid.CellStyle>
    //                    <Style TargetType = "{x:Type DataGridCell}" >
    //                        < Style.Triggers >
    //                            < Trigger Property="IsSelected" Value="True">
    //                                <Setter Property = "local2:DataGridAttachedProperties.IsCellSelected" Value="True"/>
    //                            </Trigger>
    //                            <Trigger Property = "IsSelected" Value="False">
    //                                <Setter Property = "local2:DataGridAttachedProperties.IsCellSelected" Value="False"/>
    //                            </Trigger>
    //                        </Style.Triggers>
    //                    </Style>
    //                </DataGrid.CellStyle>
    //                <DataGrid.ItemContainerStyle>
    //                    <Style TargetType = "{x:Type DataGridRow}" >
    //                        < Style.Triggers >
    //                            < Trigger Property="local2:DataGridAttachedProperties.IsCellSelected" Value="True">
    //                                <Setter Property = "BorderThickness" Value="2"/>
    //                                <Setter Property = "BorderBrush" Value="Red"/>
    //                                <Setter Property = "Background" Value="Yellow"/>
    //                                <Setter Property = "Opacity" Value="0.7"/>
    //                            </Trigger>
    //                        </Style.Triggers>
    //                    </Style>
    //                </DataGrid.ItemContainerStyle>

    /// </summary>
    public class DataGridAttachedProperties
    {
        public static bool GetIsCellSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCellSelectedProperty);
        }
        public static void SetIsCellSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCellSelectedProperty, value);
        }
        public static readonly DependencyProperty IsCellSelectedProperty =
            DependencyProperty.RegisterAttached("IsCellSelected", typeof(bool), typeof(DataGridAttachedProperties), new UIPropertyMetadata(false,
            (o, e) =>
            {
                if (o is DataGridCell)
                {
                    DataGridRow row = VisualTreeHelperEx.FindVisualParent<DataGridRow>(o as DataGridCell);
                    row.SetValue(DataGridAttachedProperties.IsCellSelectedProperty, e.NewValue);
                }
            }));
    }
    public class VisualTreeHelperEx
    {
        public static T FindVisualParent<T>(DependencyObject child)
        where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }
    }
}
