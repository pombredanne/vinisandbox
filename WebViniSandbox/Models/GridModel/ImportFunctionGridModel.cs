using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class ImportFunctionGridModel : GridModel<import_function>
    {
        public ImportFunctionGridModel()
        {
            Column.For(p => p.import_library.name).Named("Biblioteca");
            Column.For(p => p.name).Named("Nome");
            Column.For(p => p.offset).Named("Offset");
        }
    }
}