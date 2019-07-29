using System;
using System.Collections.Generic;
using System.Text;

namespace App.Projects
{
    public class NullableHelper
    {
        public static void SetNull<T>(T obj)
        {
            var properties = typeof(T).GetProperties();
            foreach(var property in properties)
            {
                if(property.PropertyType == typeof(int?))
                {
                    if((int?)property.GetValue(obj, null) == 0)
                    {
                        property.SetValue(obj, null);
                    }
                }
            }
        }
    }
}
