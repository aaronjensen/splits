using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Spark.Web.Mvc;

namespace Splits.Web.Spark
{
  public abstract class SplitsSparkView<T> : SparkView<T> where T : class
  {
  }

  public abstract class SplitsSparkView : SparkView
  {
    HtmlHelper<object> _htmlHelper;
    
    protected override void CreateHelpers()
    {
      this.Html = new HtmlHelper<object>(base.ViewContext, this);
    }

    public new HtmlHelper<object> Html
    {
      get { return _htmlHelper; }
      set { _htmlHelper = value; base.Html = value; }
    }
  }
}
