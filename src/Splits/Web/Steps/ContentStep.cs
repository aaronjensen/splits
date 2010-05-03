using System;
using System.Collections.Generic;

namespace Splits.Web.Steps
{
  public class ContentStep : Step
  {
    public Func<string> OutputFactory { get; set; }

    public ContentStep(string output)
      : this(() => output)
    {
    }

    public ContentStep(Func<string> outputFactory)
    {
      OutputFactory = outputFactory;
    }
  }
}
