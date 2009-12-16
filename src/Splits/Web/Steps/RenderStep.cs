using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splits.Web.Steps
{
  public class RenderStep : IStep
  {
    public string Text { get; set; }

    public RenderStep(string text)
    {
      Text = text;
    }
  }
}
