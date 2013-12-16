using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class ExportFunctionGridModel : GridModel<export_function>
    {
        public ExportFunctionGridModel()
        {
            Column.For(p => p.name).Named("Nome");
        }
    }
}