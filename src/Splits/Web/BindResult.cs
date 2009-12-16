using System.Collections.Generic;

namespace Splits.Web
{
  public class BindResult
  {
    public List<ConvertProblem> Problems = new List<ConvertProblem>();
    public object Value;
  }
}