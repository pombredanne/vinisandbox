using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.UI.Grid;
using ViniSandbox.Models;
using WebViniSandbox.Helpers;

namespace WebViniSandbox.Models
{
    public class FileGridModel : GridModel<file>
    {
        public FileGridModel()
        {         
            Column.For(a => "<a href=\"/File/DownloadFile/" + a.id + "\">" + a.name  + "</a>").Encode(false).Named("Nome");
            Column.For(a => a.source).Named("Fonte");
            //Column.For(a => a.date.Value.ToString("dd/MM/yyyy hh:mm"));
            //Column.For(a => a.file_detail.type);
            Column.For(a => a.file_detail.md5).Named("MD5");
            Column.For(a => "<a href=\"/Analysis/Index?id_file=" + a.id + "\">" + a.file_detail.analyses.Count() + "</a>").Named("Análises").Encode(false);
            Column.For(a =>
                TagBuilders.ImageButtonGrid("javascript:callpdetailsfile(true," + a.id + ")", "/Content/ico/details.png", "Detalhes") +

                TagBuilders.ImageButtonGrid("javascript:callpfmaliciousfile(" + a.id + ", true," + ((a.file_detail.malicious.HasValue && a.file_detail.malicious.Value)?"true":"false") + 
                    ")", a.file_detail.malicious.HasValue && a.file_detail.malicious.Value ? "/Content/ico/malicious.png" : "/Content/ico/malicious_disable.png", "Malicioso") +
                TagBuilders.ImageButtonGrid("javascript:callpfsendantivirusfile(" + a.id + ",false)", "/Content/ico/send.png", "Enviar Amostra") +
                TagBuilders.ImageButtonGrid("javascript:callpfreanalizefile(" + a.id + ",false)", "/Content/ico/redo.png", "Reanalisar")
                //enviar empresa de antivirus
                //malicioso                
                //reanalisar
                //Numero de analises
            ).Encode(false).Sortable(false).Named("Ações");
        }
    }
}