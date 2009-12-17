using Splits.Application;

namespace Bookstore.Application.Queries
{
  public class TestQuery : IQuery<string>
  {
    public int Bar { get; set; }
  }
}