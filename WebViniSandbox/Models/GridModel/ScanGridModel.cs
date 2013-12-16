using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class ScanGridModel : GridModel<antivirus_scan>
    {
        public ScanGridModel()
        {
            Column.For(p => p.antivirus.name);
            Column.For(p => p.av_version);
            Column.For(p => p.av_last_update.HasValue?p.av_last_update.Value.ToString():"");
            Column.For(p => p.result);
        }
    }
}