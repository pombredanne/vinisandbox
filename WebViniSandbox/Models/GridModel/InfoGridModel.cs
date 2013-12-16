using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebViniSandbox.Models.ViewModel;
using MvcContrib.UI.Grid;

namespace WebViniSandbox.Models.GridModel
{
    public class InfoGridModel : GridModel<Info>
    {
        public InfoGridModel()
        {            
            Column.For(p => p.Nome).Named("").Sortable(false);
            Column.For(p => p.Nome == "SHA512" ? (String.Format("<span style='font-size:xx-small;'>{0}</span>", p.Valor)) : p.Valor).Named("").Sortable(false).Encode(false);
        }
    }
}