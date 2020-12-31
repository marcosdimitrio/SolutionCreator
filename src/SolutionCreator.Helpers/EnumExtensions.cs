using System;
using System.ComponentModel;
using System.Reflection;

namespace SolutionCreator.Helpers
{
    public static class EnumExtensions
    {

        public static string Description(this Enum value)
        {
            //http://stackoverflow.com/questions/1970594/enums-or-tables#7004914
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

    }
}
