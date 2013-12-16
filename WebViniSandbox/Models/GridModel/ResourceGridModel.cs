using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class ResourceGridModel : GridModel<resource>
    {
        public ResourceGridModel()
        {
            Column.For(p => p.name);
            Column.For(p => p.size);
            Column.For(p => p.language);
            Column.For(p => p.resource_type.name);
        }       
    }
}