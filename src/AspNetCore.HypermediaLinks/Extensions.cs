using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static System.Reflection.BindingFlags;

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

        private const string ControllerSuffix = "Controller";
        public static string GetControllerName<T>(this Type t) where T : ControllerBase
        {
            return t.Name.Replace(ControllerSuffix, string.Empty);
        }

        public static string GetActionName(this LambdaExpression action)
        {

            var member = action.Body as MemberExpression;
            if (member == null)
            {
                member = (action.Body as UnaryExpression)?.Operand as MemberExpression;
            }
            if (member == null)
            {
                throw new ArgumentException("Action must be a member expression.");
            }
            return member.Member.Name;

        }

        public static RouteValueDictionary GetRouteValues(this LambdaExpression action)
        {
            var mce = action.Body as MethodCallExpression;
            if (mce?.Object == null)
            {
                throw new ArgumentNullException("Action Method expression is empty");
            }
            var mps = mce.Method.GetParameters();
            var routeValues = new RouteValueDictionary();

            for (int i = 0; i < mps.Length; i++)
            {
                var argVal = mce.Arguments[i].GetArgumentValue();

                routeValues.TryAdd(mps[i].Name, argVal);

            }
            return routeValues;
        }

        public static object GetArgumentValue(this Expression exp)
        {
            object argumentValue;

            if (exp.NodeType == ExpressionType.Constant)
            {
                argumentValue = ((ConstantExpression)exp).Value;
            }
            else
            {
                var tce = Expression.Convert(exp, typeof(object));
                argumentValue = Expression.Lambda<Func<object>>(tce, null).Compile().Invoke();
            }
            return argumentValue;
        }
    }
}
