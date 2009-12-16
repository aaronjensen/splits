using System.Collections.Generic;
using System.Linq;

namespace Splits.Web
{
  public class BindResult
  {
    public List<ConvertProblem> Problems = new List<ConvertProblem>();
    public object Value;
    public bool WasSuccessful { get { return !Problems.Any(); } }
  }
}