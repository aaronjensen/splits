using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Bookstore.Application.Model;
using Spark.Web.Mvc.Wrappers;

namespace BookStore.WebApp
{
  public interface ICache
  {
    Bookstore.Application.Model.BookStore BookStore { get; set; }
  }

  public class Cache : ICache
  {
    const string BookStoreKey = "__BookStore__";

    readonly ICacheProvider _cacheProvider;

    public Cache(ICacheProvider cacheProvider)
    {
      _cacheProvider = cacheProvider;
    }

    public Bookstore.Application.Model.BookStore BookStore 
    { 
      get
      {
        return _cacheProvider.Get<Bookstore.Application.Model.BookStore>(BookStoreKey);
      }

      set
      {
        _cacheProvider.Put(BookStoreKey, value);
      }
    }
  }

  public interface ICacheProvider
  {
    T Get<T>(string key) where T : class;
    void Put<T>(string key, T item) where T : class;
  }
  
  public class HttpCacheProvider : ICacheProvider
  {
    public T Get<T>(string key) where T : class
    {
      return HttpContext.Current.Cache.Get(key) as T;
    }

    public void Put<T>(string key, T item) where T : class
    {
      HttpContext.Current.Cache.Insert(key, item);
    }
  }
}
