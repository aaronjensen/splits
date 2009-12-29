using System;
using System.Collections.Generic;
using System.Configuration;

namespace Splits.Configuration
{
  public class SplitsSectionHandler : ConfigurationSection, ISplitsSettings
  {
    [ConfigurationProperty("defaultViewNamespace")]
    public string DefaultViewNamespace
    {
      get; set;
    }
  }

  public interface ISplitsSettings
  {
    string DefaultViewNamespace { get; }
  }

  public class SplitsSettings : ISplitsSettings
  {
    public string DefaultViewNamespace
    {
      get { return "Splits.Views"; }
    }
  }
}
