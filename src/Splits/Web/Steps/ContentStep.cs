using System;
using System.Collections.Generic;

namespace Splits.Web.Steps
{
  public class ContentStep : IStep
  {
    public string Output { get; set; }

    public ContentStep(string output)
    {
      Output = output;
    }
  }
}
