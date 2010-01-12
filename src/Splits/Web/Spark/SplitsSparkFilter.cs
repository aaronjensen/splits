using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Spark.Web.Mvc.Descriptors;
using System.Linq;

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
      foreach (var location in PotentialLocationsInternal(locations, extra).Distinct())
      {
        yield return location;
      }
    }

    public IEnumerable<string> PotentialLocationsInternal(IEnumerable<string> locations, IDictionary<string, object> extra)
    {
      foreach (var location in locations.Select(l => l.Replace(@"/", @"\")))
      {
        if (extra.ContainsKey("urlStrongPath"))
        {
          if (location.Contains("/DisplayTemplates/"))
            yield return location.Replace("/DisplayTemplates", "").Replace(".spark", ".display.spark");
          if (location.Contains("/EditorTemplates/"))
            yield return location.Replace("/EditorTemplates", "").Replace(".spark", ".editor.spark");
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
