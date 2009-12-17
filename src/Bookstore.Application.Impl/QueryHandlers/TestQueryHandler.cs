using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Queries;
using Splits.Application;

namespace Bookstore.Application.Impl.QueryHandlers
{
  public class TestQueryHandler : IQueryHandler<TestQuery, string>
  {
    public string Handle(TestQuery query)
    {
      if (query.Bar > 0)
      {
        return "Bar is > 0, it's " + query.Bar;
      }

      return "uh oh, Bar is <= 0";
    }
  }
}
