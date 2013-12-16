using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class DnsGridModel : GridModel<dns>
    {
        public DnsGridModel()
        {
            Column.For(p => p.domain);
        }
    }
}