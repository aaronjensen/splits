using System.Collections.Generic;

namespace Bookstore.WebApp.Framework
{
  public class BindResult
  {
    public List<ConvertProblem> Problems = new List<ConvertProblem>();
    public object Value;
  }
}