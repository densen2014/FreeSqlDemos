using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AME.Common
{
    public class RequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "该字段不能为空值！");
            if (string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "该字段不能为空字符串！");
            return new ValidationResult(true, null);
        }
    }

    public class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex emailReg = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");

            if (!String.IsNullOrEmpty(value.ToString()))
            {
                if (!emailReg.IsMatch(value.ToString()))
                {
                    return new ValidationResult(false, "邮箱地址不准确！");
                }
            }
            return new ValidationResult(true, null);
        }
    }

}
