using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeyVaultManagement
{
    public static class MvcHtmlStringExtension
    {
        public static MvcHtmlString Disable(this MvcHtmlString helper, bool value)
        {
            if (helper == null)
                throw new ArgumentNullException();

            if (value)
            {
                string html = helper.ToString();
                int startIndex = html.IndexOf('>');

                html = html.Insert(startIndex, " disabled=\"disabled\"");
                return MvcHtmlString.Create(html);
            }

            return helper;
        }

        public static MvcHtmlString Readonly(this MvcHtmlString helper, bool value)
        {
            if (helper == null)
                throw new ArgumentNullException();

            if (value)
            {
                string html = helper.ToString();
                int startIndex = html.IndexOf('>');

                html = html.Insert(startIndex, " readonly=\"readonly\"");
                return MvcHtmlString.Create(html);
            }

            return helper;
        }
    }
}