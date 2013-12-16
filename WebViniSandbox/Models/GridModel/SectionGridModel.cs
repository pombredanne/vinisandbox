using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class SectionGridModel : GridModel<section>
    {
        public SectionGridModel()
        {
            Column.For(p => p.name);
            Column.For(p => p.virtual_address);
            Column.For(p => p.virtual_size);
            Column.For(p => p.raw_size);
            Column.For(p => p.md5);
            Column.For(p => p.suspicious);
        }
    }
}