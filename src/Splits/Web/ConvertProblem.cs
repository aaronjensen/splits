using System;
using System.Reflection;

namespace Splits.Web
{
  public class ConvertProblem
  {
    public object Item { get; set; }
    public PropertyInfo Property { get; set; }
    public ParameterInfo Parameter { get; set; }
    public object Value { get; set; }
    public Exception Exception { get; set; }

    public override string ToString()
    {
      return
        @"Item type:       {0}
Property:        {1}
Property Type:   {2}
Attempted Value: {3}
Exception:
{4} 
"
          .ToFormat(
          ((Item != null) ? Item.GetType().FullName : "(null)"),
          Property != null ? Property.Name : Parameter.Name,
          Property != null ? Property.PropertyType : Parameter.ParameterType,
          Value,
          Exception);
    }
  }
}