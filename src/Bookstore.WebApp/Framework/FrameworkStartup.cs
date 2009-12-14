using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookstore.WebApp.Framework
{
  public interface IStepProvider
  {
    IEnumerable<IStep> GetStepsForGet(Type urlType);
    IEnumerable<IStep> GetStepsForPost(Type urlType);
  }

  public class StepProvider : IStepProvider
  {
    readonly IEnumerable<IRule> _rules;

    public StepProvider(IEnumerable<IRule> rules)
    {
      _rules = rules;
    }

    public IEnumerable<IStep> GetStepsForGet(Type urlType)
    {
      foreach (var rule in _rules)
      {
        if (rule.ShouldApply(urlType))
        {
          foreach (var step in rule.OnAny(urlType))
          {
            yield return step;
          }
          foreach (var step in rule.OnGet(urlType))
          {
            yield return step;
          }
        }
      }
    }

    public IEnumerable<IStep> GetStepsForPost(Type urlType)
    {
      foreach (var rule in _rules)
      {
        if (rule.ShouldApply(urlType))
        {
          foreach (var step in rule.OnAny(urlType))
          {
            yield return step;
          }
          foreach (var step in rule.OnPost(urlType))
          {
            yield return step;
          }
        }
      }
    }
  }

  public class FrameworkStartup
  {
    readonly IEnumerable<IRule> _rules;

    public FrameworkStartup(IEnumerable<IRule> rules)
    {
      _rules = rules;
    }

    public void Start()
    {
    }
  }
}
