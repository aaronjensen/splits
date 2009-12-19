using System;
using System.Collections.Generic;
using Spark.Web.Mvc;

namespace Splits.Web.Spark
{
  public abstract class SplitsSparkView<T> : SparkView<T> where T : class
  {
    // public Urls.Root Root { get { return Urls.root; } }
  }

  public abstract class SplitsSparkView : SparkView
  {
    // public Urls.Root Root { get { return Urls.root; } }
  }
}
