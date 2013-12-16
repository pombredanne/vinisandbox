using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class ComputerEventGridModel : GridModel<computer_event>
    {
        public ComputerEventGridModel()
        {
            Column.For(p => p.operation);
            Column.For(p => p.path);
            Column.For(p => p.result);
            Column.For(p => p.detail);
        }
    }
}