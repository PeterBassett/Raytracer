using System;
using System.ComponentModel;

namespace Raytracer.MathTypes.Converters
{
    class VectorConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (typeof(string) == sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return Vector.Parse(value as string);

            return null;
        }
    }
}
