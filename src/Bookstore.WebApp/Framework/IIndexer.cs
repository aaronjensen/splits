namespace Bookstore.WebApp.Framework
{
  public interface IIndexer<TKey, TValue>
  {
    TValue this[TKey key] { get; set; }
  }
}