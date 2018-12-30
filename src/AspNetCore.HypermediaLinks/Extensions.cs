using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using  static System.Reflection.BindingFlags;

namespace AspNetCore.HypermediaLinks
{
    public static class Extensions
    {
        /// <summary>
        /// at present this support Ienumerable
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsLinkSupportGenericType(this PropertyInfo p)
        {
            var type = p.PropertyType;
            if (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                type.GetGenericTypeDefinition() == typeof(IList<>))
               )
            {
                var arg = type.GetGenericArguments().FirstOrDefault();
                if (arg != null)
                {
                    return arg.IsSubclassOf(typeof(HyperMediaSupportModel));
                }
            }

            return false;
        }

        public static bool IsLinkSupportArray(this PropertyInfo p)
        {
            return p.PropertyType.IsArray && p.PropertyType.GetElementType().IsSubclassOf(typeof(HyperMediaSupportModel));
        }

        public static bool IsLinkSupportModel(this PropertyInfo p)
        {
            return p.PropertyType.IsSubclassOf(typeof(HyperMediaSupportModel));
        }

        public static StringBuilder ReplaceValues(this StringBuilder template, object obj) {

            var props = obj.GetType().GetProperties(Instance | Public).ToList();
            //TODO validate the input value count
            props.ForEach(p =>
            {
                template.Replace($"{{{p.Name}}}", p.GetValue(obj)?.ToString());
            });
            return template;
        }
    }
}
