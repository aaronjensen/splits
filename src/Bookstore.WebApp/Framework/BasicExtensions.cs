using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bookstore.WebApp.Framework
{
  public static class BasicExtensions
  {
    public static void TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
                                            Action<TValue> action)
    {
      TValue value;
      if (dictionary.TryGetValue(key, out value))
      {
        action(value);
      }
    }

    public static void Fill<T>(this IList<T> list, T value)
    {
      if (list.Contains(value)) return;
      list.Add(value);
    }

    public static string Join(this IEnumerable<string> strings, string separator)
    {
      string[] array = strings.ToArray();
      return string.Join(separator, array);
    }

    public static bool IsEmpty(this string stringValue)
    {
      return string.IsNullOrEmpty(stringValue);
    }

    public static bool IsNotEmpty(this string stringValue)
    {
      return !string.IsNullOrEmpty(stringValue);
    }

    public static bool ToBool(this string stringValue)
    {
      if (string.IsNullOrEmpty(stringValue)) return false;

      return bool.Parse(stringValue);
    }

    public static string ToFormat(this string stringFormat, params object[] args)
    {
      return String.Format(stringFormat, args);
    }

    public static string If(this string html, Expression<Func<bool>> modelBooleanValue)
    {
      return GetBooleanPropertyValue(modelBooleanValue) ? html : string.Empty;
    }

    public static string IfNot(this string html, Expression<Func<bool>> modelBooleanValue)
    {
      return !GetBooleanPropertyValue(modelBooleanValue) ? html : string.Empty;
    }

    private static bool GetBooleanPropertyValue(Expression<Func<bool>> modelBooleanValue)
    {
      var prop = modelBooleanValue.Body as MemberExpression;
      if (prop != null)
      {
        var info = prop.Member as PropertyInfo;
        if (info != null)
        {
          return modelBooleanValue.Compile().Invoke();
        }
      }
      throw new ArgumentException(
          "The modelBooleanValue parameter should be a single property, validation logic is not allowed, only 'x => x.BooleanValue' usage is allowed, if more is needed do that in the Controller");
    }

    public static VALUE Get<KEY, VALUE>(this IDictionary<KEY, VALUE> dictionary, KEY key)
    {
      return dictionary.Get(key, default(VALUE));
    }

    public static VALUE Get<KEY, VALUE>(this IDictionary<KEY, VALUE> dictionary, KEY key, VALUE defaultValue)
    {
      if (dictionary.ContainsKey(key)) return dictionary[key];
      return defaultValue;
    }

    public static bool Exists<T>(this IEnumerable<T> values, Func<T, bool> evaluator)
    {
      return values.Count(evaluator) > 0;
    }

    [DebuggerStepThrough]
    public static IEnumerable<T> Each<T>(this IEnumerable<T> values, Action<T> eachAction)
    {
      foreach (T item in values)
      {
        eachAction(item);
      }

      return values;
    }

    [DebuggerStepThrough]
    public static IEnumerable Each(this IEnumerable values, Action<object> eachAction)
    {
      foreach (object item in values)
      {
        eachAction(item);
      }

      return values;
    }

    [DebuggerStepThrough]
    public static int IterateFromZero(this int maxCount, Action<int> eachAction)
    {
      for (int idx = 0; idx < maxCount; idx++)
      {
        eachAction(idx);
      }

      return maxCount;
    }

    public static bool HasCustomAttribute<ATTRIBUTE>(this MemberInfo member)
        where ATTRIBUTE : Attribute
    {
      return member.GetCustomAttributes(typeof(ATTRIBUTE), false).Any();
    }

    public static bool IsNullable(this Type theType)
    {
      return (!theType.IsValueType) || theType.IsNullableOfT();
    }

    public static bool IsNullableOfT(this Type theType)
    {
      return theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
    }

    public static bool IsNullableOf(this Type theType, Type otherType)
    {
      return theType.IsNullableOfT() && theType.GetGenericArguments()[0].Equals(otherType);
    }

    public static IList<T> AddMany<T>(this IList<T> list, params T[] items)
    {
      return list.AddRange(items);
    }

    public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
      items.Each(t => list.Add(t));
      return list;
    }

    public static string UrlEncoded(this object target)
    {
      //properly encoding URI: http://blogs.msdn.com/yangxind/default.aspx
      return target != null ? Uri.EscapeDataString(target.ToString()) : string.Empty;
    }


    [DebuggerStepThrough]
    public static IEnumerable<U> WhereMatching<U>(this IEnumerable enumerable, Func<U, bool> predicate)
        where U : class
    {
      return enumerable.OfType<object>()
          .Where(x => x is U)
          .Select(x => x as U)
          .Where(predicate);
    }

    public static IEnumerable<T> SelectTo<T>(this IEnumerable enumerable)
    {
      foreach (object o in enumerable)
      {
        if (o is T)
        {
          yield return (T)o;
        }
      }
    }
  }
}
