using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDP_EC.App.Converters
{
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("F2", culture); // Formatea a dos decimales
            }
            return string.Empty; // Retorna cadena vacía si el valor no es decimal
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Any, culture, out var result))
            {
                return result; // Convierte de vuelta a decimal
            }
            return 0m; // Retorna 0 si la conversión falla
        }
    }
}
