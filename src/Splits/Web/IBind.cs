using System;
using System.Collections.Generic;

namespace Splits.Web
{
  public interface IBind<F, T> 
  {
    void Bind(StepContext stepContext, F from, T to);
  }
}
