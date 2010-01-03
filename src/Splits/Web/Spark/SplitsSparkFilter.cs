using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Spark.Web.Mvc.Descriptors;

namespace Splits.Web.Spark
{
  public class SplitsSparkFilter : DescriptorFilterBase
  {
    public override void ExtraParameters(ControllerContext context, IDictionary<string, object> extra)
    {
      if (!context.RequestContext.IsUrlStrongRequest())
        return;
      extra["urlStrongPath"] = context.RequestContext.UrlStrongPathFromRoute();
    }

    public override IEnumerable<string> PotentialLocations(IEnumerable<string> locations, IDictionary<string, object> extra)
    {
      foreach (var location in locations)
      {
        if (extra.ContainsKey("urlStrongPath"))
        {
          yield return location.Replace(@"Splits\", extra["urlStrongPath"] + @"\");
        }
        else
        {
          yield return location;
        }
      }
    }
  }
}
