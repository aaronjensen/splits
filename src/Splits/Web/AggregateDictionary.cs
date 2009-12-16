using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace Splits.Web
{
  public class AggregateDictionary : IDictionary<string, object>
  {
    private readonly IList<Func<string, object>> _locators = new List<Func<string, object>>();

    public AggregateDictionary()
    {
    }

    public AggregateDictionary(params Func<string, object>[] locators)
    {
      locators.Each(AddLocator);
    }

    public AggregateDictionary(RequestContext requestContext)
    {
      AddLocator(key =>
      {
        object found;
        requestContext.RouteData.Values.TryGetValue(key, out found);
        return found;
      });
      AddLocator(key => requestContext.HttpContext.Request[key]);
      AddLocator(key => requestContext.HttpContext.Request.Files[key]);
      AddLocator(key => requestContext.HttpContext.Request.Headers[key]);
      
    }

    public bool TryGetValue(string key, out object value)
    {
      value = null;

      foreach (var locator in _locators)
      {
        value = locator(key);

        if (value != null)
        {
          return true;
        }
      }

      return false;
    }

    public object this[string key]
    {
      get
      {
        object value;
        TryGetValue(key, out value);
        return value;
      }
      set { throw new NotImplementedException(); }
    }

    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    void ICollection<KeyValuePair<string, object>>.Clear()
    {
      throw new NotImplementedException();
    }

    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
    {
      throw new NotImplementedException();
    }

    int ICollection<KeyValuePair<string, object>>.Count { get { throw new NotImplementedException(); } }

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly { get { throw new NotImplementedException(); } }

    bool IDictionary<string, object>.ContainsKey(string key)
    {
      object returnValue;
      return TryGetValue(key, out returnValue);
    }

    void IDictionary<string, object>.Add(string key, object value)
    {
      throw new NotImplementedException();
    }

    bool IDictionary<string, object>.Remove(string key)
    {
      throw new NotImplementedException();
    }

    ICollection<string> IDictionary<string, object>.Keys { get { throw new NotImplementedException(); } }

    ICollection<object> IDictionary<string, object>.Values { get { throw new NotImplementedException(); } }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void AddLocator(Func<string, object> locator)
    {
      _locators.Add(locator);
    }
  }
}
