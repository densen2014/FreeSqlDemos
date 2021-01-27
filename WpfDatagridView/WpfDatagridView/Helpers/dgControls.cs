using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AME.Helpers
{
    public class dgControls
    {
        public static void dgv回车往右简单版(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var u = e.OriginalSource as UIElement;
            if ((e.Key == Key.Enter) || (e.Key == Key.Return) && u != null)
            {
                e.Handled = true;
                u.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }


        public static void dgv回车往右并开启编辑(object sender, KeyEventArgs e)
        {

            //just accept enter key
            if (e.Key != Key.Enter) return;
            var dgv = sender as DataGrid;
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            //here we just find the cell got focused ...
            //then we can use the cell key down or key up
            // iteratively traverse the visual tree
            while ((dep != null) && !(dep is DataGridCell) && !(dep is DataGridColumnHeader))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            if (dep is DataGridCell)
            {
                //cancel if datagrid in edit mode
                dgv.CancelEdit();
                //get current cell
                DataGridCell cell = dep as DataGridCell;
                //deselect current cell
                //以后再调试 cell.IsSelected = false;  报错 SelectionUnit 为 FullRow 时，无法更改单元格选择。SelectionUnit = Cell才能设置 ”
                //find next right cell
                var nextCell = cell.PredictFocus(FocusNavigationDirection.Right);
                //if next right cell null go for find next ro first cell
                if (nextCell == null)
                {
                    DependencyObject nextRowCell;
                    nextRowCell = cell.PredictFocus(FocusNavigationDirection.Down);
                    //if next row is null so we have no more row Return;
                    if (nextRowCell == null) return;
                    //we do this because we cant use FocusNavigationDirection.Next for function PredictFocus
                    //so we have to find it this way
                    while ((nextRowCell as DataGridCell).PredictFocus(FocusNavigationDirection.Left) != null)
                        nextRowCell = (nextRowCell as DataGridCell).PredictFocus(FocusNavigationDirection.Left);
                    //set new cell as next cell
                    nextCell = nextRowCell;
                }
                //change current cell
                dgv.CurrentCell = new DataGridCellInfo(nextCell as DataGridCell);
                //change selected cell


                (nextCell as DataGridCell).IsSelected = true;
                // start edit mode
                dgv.BeginEdit();
            }
            //handl the default action of keydown
            e.Handled = true;
        }
        public static void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }
    }

    public class DataGridNumericColumn : DataGridTextColumn
    {
        protected override object PrepareCellForEdit(System.Windows.FrameworkElement editingElement, System.Windows.RoutedEventArgs editingEventArgs)
        {
            TextBox edit = editingElement as TextBox;
            edit.PreviewTextInput += OnPreviewTextInput;

            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch
            {
                // Show some kind of error message if you want

                // Set handled to true
                e.Handled = true;
            }
        }
    }

}
