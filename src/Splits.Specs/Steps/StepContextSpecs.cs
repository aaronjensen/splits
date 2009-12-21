using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using Machine.Specifications;
using Machine.UrlStrong;
using Rhino.Mocks;
using Splits.Application;
using Splits.Web;

namespace Splits.Specs.Steps
{
  [Subject(typeof(StepContext))]
  public class when_adding_a_query_result_that_has_already_been_added
  {
    Establish context = () =>
    {
      stepContext = new StepContext(FakeRequestContext.New(), typeof(FakeUrl));
      query = new FakeQuery();
      stepContext.AddQuery(query, 3, "");
    };

    Because of = () =>
      exception = Catch.Exception(() => stepContext.AddQuery(query, 3, ""));

    It should_fail = () =>
      exception.ShouldNotBeNull();

    static StepContext stepContext;
    static FakeQuery query;
    static Exception exception;
  }
  
  public class FakeQuery : Query<int>
  {
    
  }

  public class FakeUrl : ISupportGet
  {
    public string ToParameterizedUrl()
    {
      return "/";
    }

    public IEnumerable<KeyValuePair<string, object>> Parameters
    {
      get { return new KeyValuePair<string, object>[0]; }
    }
  }

  public class FakeRequestContext
  {
    public static RequestContext New()
    {
      var context = MockRepository.GenerateStub<HttpContextBase>();
      var routeData = new RouteData();

      return new RequestContext(context, routeData);
    }
  }

  class StepContextSpecs
  {
  }
}
