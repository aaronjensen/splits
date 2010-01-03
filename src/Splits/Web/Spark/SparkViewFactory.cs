// Copyright 2008-2009 Louis DeJardin - http://whereslou.com
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

using Spark;
using Spark.Web.Mvc;
using Spark.Web.Mvc.Descriptors;

using Splits.Configuration;

namespace Splits.Web.Spark
{
  public class SparkViewFactory : ISparkServiceInitialize
  {
    readonly ISplitsSettings _splitsSettings;
    ISparkViewEngine _engine;
    ICacheServiceProvider _cacheServiceProvider;
    ISparkSettings _settings;

    public SparkViewFactory()
    {
      _splitsSettings = ((ISplitsSettings)ConfigurationManager.GetSection("splits")) ?? new SplitsSettings();
      _settings = ((ISparkSettings)ConfigurationManager.GetSection("spark")) ?? new SparkSettings {
        PageBaseType = typeof(SplitsSparkView).FullName
      };
      var services = SparkEngineStarter.CreateContainer(_settings);
      services.AddFilter(new SplitsSparkFilter());
      Initialize(services);
      SparkEngineStarter.RegisterViewEngine(services);
    }

    public void Initialize(ISparkServiceContainer container)
    {
      _settings = container.GetService<ISparkSettings>();
      Engine = container.GetService<ISparkViewEngine>();
      CacheServiceProvider = container.GetService<ICacheServiceProvider>();
    }

    public SplitsViewEngineResult FindView(RequestContext requestContext, string location, string viewName, string masterName, bool findDefaultMaster, bool searchParentFolders)
    {
      var locations = new[] { location };
      if (searchParentFolders)
      {
        locations = ItselfAndAllParentFolders(location).
          Union(new[] { "Shared" }).
          ToArray();
      }
      return FindView(requestContext, locations, viewName, masterName, findDefaultMaster);
    }

    public SplitsViewEngineResult FindView(RequestContext requestContext, IEnumerable<string> locations, string viewName, string masterName, bool findDefaultMaster)
    {
      var descriptor = new SparkViewDescriptor();
      var viewLocations = locations.Select(location => Path.Combine(location, viewName + ".spark")).Select(path => path.Replace(@"/", @"\"));
      var searchLocations = viewLocations;
      var masterLocations = new[] { @"Layouts\Application.spark", @"Shared\Application.spark"  };
      if (findDefaultMaster)
      {
        searchLocations = searchLocations.Union(masterLocations);
      }
      descriptor.TargetNamespace = _splitsSettings.DefaultViewNamespace;

      var foundViewOtherThanMaster = false;
      foreach (var template in searchLocations.Where(template => Engine.ViewFolder.HasView(template)))
      {
        descriptor.Templates.Add(template);
        foundViewOtherThanMaster = foundViewOtherThanMaster || !masterLocations.Contains(template);
      }

      if (!foundViewOtherThanMaster)
      {
        return new SplitsViewEngineResult(viewLocations);
      }

      var entry = Engine.CreateEntry(descriptor);
      return new SplitsViewEngineResult(CreateViewInstance(requestContext, entry), viewLocations);
    }

    public void ReleaseView(IView view)
    {
      Engine.ReleaseInstance((ISparkView)view);
    }

    static IEnumerable<string> ItselfAndAllParentFolders(string folder)
    {
      while (!String.IsNullOrEmpty(folder))
      {
        yield return folder;
        folder = Path.GetDirectoryName(folder);
      }
    }

    IView CreateViewInstance(RequestContext requestContext, ISparkViewEntry entry)
    {
      var view = (IView)entry.CreateInstance();
      if (view is SparkView)
      {
        var sparkView = (SparkView)view;
        sparkView.ResourcePathManager = Engine.ResourcePathManager;
        sparkView.CacheService = CacheServiceProvider.GetCacheService(requestContext);
      }
      return view;
    }

    ISparkViewEngine Engine
    {
      set
      {
        _engine = value;
        if (_engine != null)
        {
          _engine.DefaultPageBaseType = typeof(SplitsSparkView).FullName;
        }
      }
      get
      {
        if (_engine == null)
        {
          Engine = new SparkViewEngine(_settings);
        }
        return _engine;
      }
    }

    ICacheServiceProvider CacheServiceProvider
    {
      get { return _cacheServiceProvider ?? Interlocked.CompareExchange(ref _cacheServiceProvider, new DefaultCacheServiceProvider(), null) ?? _cacheServiceProvider; }
      set { _cacheServiceProvider = value; }
    }
  }

  public class SplitsViewEngineResult
  {
    readonly IView _view;
    readonly IEnumerable<string> _searchedLocations;

    public IView View
    {
      get { return _view; }
    }

    public IEnumerable<string> SearchedLocations
    {
      get { return _searchedLocations; }
    }

    public SplitsViewEngineResult(IEnumerable<string> searchedLocations)
      : this(null, searchedLocations)
    {
    }

    public SplitsViewEngineResult(IView view, IEnumerable<string> searchedLocations)
    {
      _view = view;
      _searchedLocations = searchedLocations;
    }
  }
}