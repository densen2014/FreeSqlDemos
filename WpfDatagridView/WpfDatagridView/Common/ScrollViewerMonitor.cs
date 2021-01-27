using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace POS20
{
    public class ScrollViewerMonitor
    {
        public static ICommand GetAtEndCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(AtEndCommandProperty);
        }

        public static void SetAtEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(AtEndCommandProperty, value);
        }

        public static readonly DependencyProperty AtEndCommandProperty =
            DependencyProperty.RegisterAttached("AtEndCommand", typeof(ICommand),
                typeof(ScrollViewerMonitor), new PropertyMetadata(OnAtEndCommandChanged));

        public static void OnAtEndCommandChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            if (element != null)
            {
                element.Loaded -= element_Loaded;
                element.Loaded += element_Loaded;
            }
        }

        private static void element_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.Loaded -= element_Loaded;

            ScrollViewer scrollViewer = FindChildOfType<ScrollViewer>(element);

            if (scrollViewer == null)
            {
                throw new InvalidOperationException("ScrollViewer not found.");
            }

            scrollViewer.ScrollChanged += delegate
            {
                //滚动内容的垂直偏移量 VerticalOffset , 可以滚动的内容元素的垂直大小 ScrollableHeight
                bool atBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;

                if (atBottom)
                {
                    var atEnd = GetAtEndCommand(element);
                    if (atEnd != null)
                    {
                        atEnd.Execute(null);
                    }
                }
            };
        }

        private static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}