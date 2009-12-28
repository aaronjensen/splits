using System;
using System.Collections.Generic;
using System.Web;
using Splits.Application;

namespace Splits.Queries
{
  public class IsAuthenticatedQuerySpec : Query<bool>
  {
  }

  public class IsAuthenticatedQueryHandler : IQueryHandler<IsAuthenticatedQuerySpec, bool>
  {
    public bool Handle(IsAuthenticatedQuerySpec query)
    {
      return HttpContext.Current.User.Identity.IsAuthenticated;
    }
  }
}
