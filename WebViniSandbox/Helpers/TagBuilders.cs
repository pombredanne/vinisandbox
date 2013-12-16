using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebViniSandbox.Helpers
{
    public class TagBuilders
    {
        public static string ImageButtonGrid(string page, string imgScr, string tooltip)
        {
            return String.Format("<img src='{1}' width='16' height='16' onclick='{0}' class='linkGrid' title='{2}'/>", 
                page, imgScr, tooltip);
        }

        public static string ImageLinkGrid(string page, string imgScr, string tooltip)
        {
            return String.Format("<a href='{0}'><img src='{1}' width='16' height='16' class='linkGrid' title='{2}'/></a>",
                page, imgScr, tooltip);
        }

        public static string ConfirmRemoveLink(string page, string imgScr)
        {
            return String.Format("<a class='delete-link' href='{0}'>a</a>'", page);//"<img src='{0}' class='delete-link' href='{1}' />", imgScr, page);
        }
    }
}