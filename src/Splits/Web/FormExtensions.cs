using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Machine.UrlStrong;

namespace Splits.Web
{
  public static class FormExtensions
  {
    public static MvcForm BeginForm(this HtmlHelper htmlHelper, ISupportPost url)
    {
      return BeginForm(htmlHelper, url, FormMethod.Post);
    }

    public static MvcForm BeginForm(this HtmlHelper htmlHelper, ISupportGet url)
    {
      return BeginForm(htmlHelper, url, FormMethod.Get);
    }

    public static MvcForm BeginForm(this HtmlHelper htmlHelper, IUrl url, FormMethod method)
    {
      return BeginForm(htmlHelper, url, method, new Dictionary<string, object>());
    }

    public static MvcForm BeginForm(this HtmlHelper htmlHelper, IUrl url, FormMethod method, IDictionary<string, object> htmlAttributes)
    {
      var builder = new TagBuilder("form");
      builder.MergeAttributes(htmlAttributes);
      builder.MergeAttribute("action", url.ToString());
      builder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
      if (htmlHelper.ViewContext.ClientValidationEnabled)
      {
        builder.GenerateId("form0");
      }
      htmlHelper.ViewContext.HttpContext.Response.Write(builder.ToString(TagRenderMode.StartTag));
      var form = new MvcForm(htmlHelper.ViewContext);
      if (htmlHelper.ViewContext.ClientValidationEnabled)
      {
        htmlHelper.ViewContext.FormContext.ClientValidationEnabled = true;
        htmlHelper.ViewContext.FormContext.FormId = builder.Attributes["id"];
      }
      return form;
    }
  }
}
