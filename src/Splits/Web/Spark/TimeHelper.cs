using System;
using System.Web;

namespace Splits.Web.Spark
{
  public class TimeHelper
  {
    public TimeZoneInfo UserTimeZone
    {
      get;
      set;
    }

    public TimeHelper(HttpContextBase httpContextBase)
    {
      if (UserTimeZone == null)
      {
        UserTimeZone = TimeZoneInfo.Local;
      }
    }

    public string ToLocal(DateTime dateTime)
    {
      return TimeZoneInfo.ConvertTimeFromUtc(dateTime, UserTimeZone).ToString();
    }

    public string ToLocal(DateTime dateTime, string formatString)
    {
      return TimeZoneInfo.ConvertTimeFromUtc(dateTime, UserTimeZone).ToString(formatString);
    }

    public string ToTimeOfDay(TimeSpan timeSpan)
    {
      return new DateTime(timeSpan.Ticks).ToShortTimeString().ToLower();
    }
  }
}