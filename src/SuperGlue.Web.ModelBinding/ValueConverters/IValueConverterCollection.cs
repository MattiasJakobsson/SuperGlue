using System;
using System.Collections.Generic;

namespace SuperGlue.Web.ModelBinding.ValueConverters
{
    public interface IValueConverterCollection : IEnumerable<IValueConverter>
    {
        bool CanConvert(Type destinationType);
        object Convert(Type destinationType, object value);
    }
}