using System;
using System.Collections.Generic;
using Splits.Application;
using System.Linq;
using Splits.Internal;

namespace Splits.Web
{
  public interface IStepProvider
  {
    IEnumerable<IStep> GetStepsForGet(Type urlType);
    IEnumerable<IStep> GetStepsForPost(Type urlType);
  }
  
  public class StepProvider : IStepProvider
  {
    readonly IEnumerable<IRule> _rules;

    public StepProvider()
    {
      _rules = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetAllInstances<IRule>();
    }

    public IEnumerable<IStep> GetStepsForGet(Type urlType)
    {
      foreach (var rule in _rules.Where(r => r.ShouldApply(urlType)))
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

    public IEnumerable<IStep> GetStepsForPost(Type urlType)
    {
      foreach (var rule in _rules.Where(r => r.ShouldApply(urlType)))
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

  public class FrameworkStartup
  {
    readonly IViewRenderer _renderer;
    readonly IConfigureSplits _configureSplits;
    readonly EventOrdering _eventOrdering;
    readonly Configurer _configurer;

    public FrameworkStartup(IConfigureSplits configureSplits, EventOrdering eventOrdering, IViewRenderer renderer)
    {
      _configureSplits = configureSplits;
      _renderer = renderer;
      _eventOrdering = eventOrdering;
      _configurer = new Configurer(_eventOrdering);
    }

    public void Start()
    {
      _configureSplits.Configure(_configurer);
    }
  }
}
