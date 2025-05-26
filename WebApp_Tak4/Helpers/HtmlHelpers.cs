using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace WebApp_Task4.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static HtmlString LastLoginTime(this IHtmlHelper html, DateTime lastLoginDate)
        {
            var now = DateTime.Now;
            var seconds = (now - lastLoginDate).TotalSeconds;

            var timeUnits = new[]
            {
                new { Value = 31536000, Unit = "year" },
                new { Value = 2592000, Unit = "month" },
                new { Value = 604800, Unit = "week" },
                new { Value = 86400, Unit = "day" },
                new { Value = 3600, Unit = "hour" },
                new { Value = 60, Unit = "minute" }
            };

            foreach (var timeUnit in timeUnits)
            {
                if (seconds >= timeUnit.Value)
                {
                    var count = (int)(seconds / timeUnit.Value);
                    var unit = count > 1 ? timeUnit.Unit + "s" : timeUnit.Unit;
                    return new HtmlString($"{count} {unit} ago");
                }
            }

            return new HtmlString("less than a minute ago");
        }
    }
}