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
using Spark.FileSystem;
using Spark.Web.Mvc;
using Spark.Web.Mvc.Wrappers;

namespace Splits.Web.Spark
{
  public class SparkViewFactory : IViewFolderContainer, ISparkServiceInitialize
  {
    ISparkViewEngine _engine;
    ICacheServiceProvider _cacheServiceProvider;

    public SparkViewFactory()
    {
    }

    public SparkViewFactory(ISparkSettings settings)
    {
      Settings = settings ?? (((ISparkSettings)ConfigurationManager.GetSection("spark")) ?? new SparkSettings());
      Settings.PageBaseType = typeof(SplitsSparkView).FullName;
    }

    public virtual void Initialize(ISparkServiceContainer container)
    {
      Settings = container.GetService<ISparkSettings>();
      Engine = container.GetService<ISparkViewEngine>();
      CacheServiceProvider = container.GetService<ICacheServiceProvider>();
    }

    public ISparkSettings Settings { get; set; }

    public ISparkViewEngine Engine
    {
      set { SetEngine(value); }
      get
      {
        if (_engine == null)
        {
          SetEngine(new SparkViewEngine(Settings));
        }
        return _engine;
      }
    }

    public void SetEngine(ISparkViewEngine engine)
    {
      _engine = engine;
      if (_engine != null)
      {
        _engine.DefaultPageBaseType = typeof(SparkView).FullName;
      }
    }

    public IViewActivatorFactory ViewActivatorFactory
    {
      get { return Engine.ViewActivatorFactory; }
      set { Engine.ViewActivatorFactory = value; }
    }

    public IViewFolder ViewFolder
    {
      get { return Engine.ViewFolder; }
      set { Engine.ViewFolder = value; }
    }

    public ICacheServiceProvider CacheServiceProvider
    {
      get { return _cacheServiceProvider ?? Interlocked.CompareExchange(ref _cacheServiceProvider, new DefaultCacheServiceProvider(), null) ?? _cacheServiceProvider; }
      set { _cacheServiceProvider = value; }
    }

    public virtual void ReleaseView(IView view)
    {
      Engine.ReleaseInstance((ISparkView)view);
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

    static IEnumerable<string> ItselfAndAllParentFolders(string folder)
    {
      while (!String.IsNullOrEmpty(folder))
      {
        yield return folder;
        folder = Path.GetDirectoryName(folder);
      }
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
      descriptor.TargetNamespace = "Split";

      var foundViewOtherThanMaster = false;
      foreach (var template in searchLocations)
      {
        if (Engine.ViewFolder.HasView(template))
        {
          descriptor.Templates.Add(template);
          foundViewOtherThanMaster = foundViewOtherThanMaster || !masterLocations.Contains(template);
        }
      }

      if (!foundViewOtherThanMaster)
      {
        return new SplitsViewEngineResult(viewLocations);
      }

      var entry = Engine.CreateEntry(descriptor);
      return new SplitsViewEngineResult(CreateViewInstance(requestContext, entry), viewLocations);
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

    void ISparkServiceInitialize.Initialize(ISparkServiceContainer container)
    {
      Initialize(container);
    }

    IViewFolder IViewFolderContainer.ViewFolder
    {
      get { return ViewFolder; }
      set { ViewFolder = value; }
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