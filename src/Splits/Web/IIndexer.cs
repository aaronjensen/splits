namespace Splits.Web
{
  public interface IIndexer<TKey, TValue>
  {
    TValue this[TKey key] { get; set; }
  }
}