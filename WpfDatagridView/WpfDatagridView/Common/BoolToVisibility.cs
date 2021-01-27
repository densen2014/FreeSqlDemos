using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;



namespace AME.Convertor
{
    public class BoolToVisibility : IValueConverter
    {
        #region "IValueConverter Members"

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                Boolean valueB = (Boolean)value;
                switch (valueB)
                {
                    case true:
                        return Visibility.Visible;
                    case false:
                        return Visibility.Collapsed;
                }
            }
            else if (value is String)
            {
                switch (value.ToString().ToLower())
                {
                    case "true":
                        return Visibility.Visible;
                    case "false":
                        return Visibility.Collapsed;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class BoolToVisibilityInvent : IValueConverter
    {
        #region "IValueConverter Members"

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            if (value is bool)
            {
                Boolean valueB = (Boolean)value;
                switch (valueB)
                {
                    case false:
                        return Visibility.Visible;
                    case true:
                        return Visibility.Collapsed;
                }
            }
            else if (value is String)
            {
                switch (value.ToString().ToLower())
                {
                    case "false":
                        return Visibility.Visible;
                    case "true":
                        return Visibility.Collapsed;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class BoolToWindowState : IValueConverter
    {
        #region "IValueConverter Members"

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                Boolean valueB = (Boolean)value;
                switch (valueB)
                {
                    case true:
                        return WindowState.Maximized;
                    case false:
                        return WindowState.Normal;
                }
            }
            else if (value is String)
            {
                switch (value.ToString().ToLower())
                {
                    case "true":
                        return WindowState.Maximized;
                    case "false":
                        return WindowState.Normal;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class BooleanConverter<T> : IValueConverter
    {
        public BooleanConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public T TrueValue { get; set; }
        public T FalseValue { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? TrueValue : FalseValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T tValue && EqualityComparer<T>.Default.Equals(tValue, TrueValue);
        }
    }
}