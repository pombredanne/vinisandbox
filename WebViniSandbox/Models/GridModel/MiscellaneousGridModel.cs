using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class MiscellaneousGridModel : GridModel<miscellaneous>
    {
        public MiscellaneousGridModel()
        {
            Column.For(p => p.type);
            Column.For(p => p.description);
        }
    }
}