using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirmaDigital.Converter
{
    internal class ByteArrayToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource imageSource = null;
            if (value != null)
            {
                byte[] bytes = (byte[])value;  
                imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
            return imageSource; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
