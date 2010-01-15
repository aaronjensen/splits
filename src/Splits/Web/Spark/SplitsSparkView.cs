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
    TimeHelper _timeHelper;
    
    protected override void CreateHelpers()
    {
      base.CreateHelpers();
      this.Html = new HtmlHelper<object>(ViewContext, this);
      _timeHelper = new TimeHelper(Context);
    }

    public new HtmlHelper<object> Html
    {
      get { return _htmlHelper; }
      set { _htmlHelper = value; base.Html = value; }
    }

    public TimeHelper Time
    {
      get { return _timeHelper; }
    }
  }
}
