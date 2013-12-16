using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;
using WebViniSandbox.Helpers;

namespace WebViniSandbox.Models
{
    public class UserGridModel : GridModel<user>
    {
        public UserGridModel()
        {
            Column.For(a => a.name);
            Column.For(a => a.nickname);
            Column.For(a => a.email);
            Column.For(a => a.admin?"Administrador":"Usuario Comun").Named("Grupo");
            Column.For(a => "<a href=\"/File/Index?id_user=" + a.id + "\">" + a.files.Count() + "</a>").Named("Arquivos Enviados").Encode(false);
            Column.For(a =>
                TagBuilders.ImageButtonGrid("javascript:callpdetailsuser(" + a.id + ")", "/Content/ico/details.png", "Detalhes") +
                TagBuilders.ImageButtonGrid("javascript:callpfedituser(" + a.id + ")", "/Content/ico/edit.png", "Editar") +
                TagBuilders.ImageButtonGrid("javascript:callpfdeleteuser(" + a.id + ")", "/Content/ico/delete.png", "Deletar")
            ).Encode(false).Sortable(false).Named("Ações");
        }
    }
}