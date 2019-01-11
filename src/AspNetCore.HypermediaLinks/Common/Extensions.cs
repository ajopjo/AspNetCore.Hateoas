using Microsoft.AspNetCore.Mvc;
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
        private const string ControllerSuffix = "Controller";
        /// <summary>
        /// at present this support Ienumerable
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsLinkSupportGenericType(this PropertyInfo p)
        {
            return p.PropertyType.IsGenericType && (p.PropertyType.GetGenericArguments().FirstOrDefault()?.IsSubclassOf(typeof(HyperMediaSupportModel)) ?? false);
        }

        public static bool IsLinkSupportArray(this PropertyInfo p)
        {
            return p.PropertyType.IsArray && p.PropertyType.GetElementType().IsSubclassOf(typeof(HyperMediaSupportModel));
        }

        public static bool IsLinkSupportModel(this PropertyInfo p)
        {
            return p.PropertyType.IsSubclassOf(typeof(HyperMediaSupportModel));
        }

        public static StringBuilder ReplaceValues(this StringBuilder template, object obj)
        {

            var props = obj.GetType().GetProperties(Instance | Public).ToList();
            //TODO validate the input value count
            props.ForEach(p =>
            {
                template.Replace($"{{{p.Name}}}", p.GetValue(obj)?.ToString());
            });
            return template;
        }

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
                if (!(action.Body is MethodCallExpression mce))
                    throw new ArgumentException("Action must be a member or method call expression.");
                return mce.Method.Name;
            }
            return member.Member.Name;

        }

        public static Dictionary<string, object> GetRouteValues(this LambdaExpression action)
        {
            var mce = action.Body as MethodCallExpression;
            if (mce?.Object == null)
            {
                throw new ArgumentNullException("Action Method expression is empty");
            }
            var mps = mce.Method.GetParameters();
            var routeValues = new Dictionary<string, object>();

            for (int i = 0; i < mps.Length; i++)
            {
                var argVal = mce.Arguments[i].GetArgumentValue();
                routeValues.TryAdd(mps[i].Name, argVal);
            }
            return routeValues;
        }

        private static object GetArgumentValue(this Expression exp)
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
