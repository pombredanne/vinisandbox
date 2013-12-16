using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.UI.Grid;
using ViniSandbox.Models;
using WebViniSandbox.Helpers;

namespace WebViniSandbox.Models
{
    public class FileDetGridModel : GridModel<file_detail>
    {
        public FileDetGridModel()
        {         
            Column.For(a => "<a href=\"/File/DownloadFile/" + a.id + "\">" + a.md5  + "</a>").Encode(false).Named("MD5");            
            //Column.For(a => a.date.Value.ToString("dd/MM/yyyy hh:mm"));
            //Column.For(a => a.file_detail.type);
            Column.For(a => "<a href=\"/File/Index?id_file_detail=" + a.id + "\">" + a.files.Count() + "</a>").Encode(false).Named("Qtde Arquivos");
            Column.For(a => "<a href=\"/Analysis/Index?id_file=" + a.files.FirstOrDefault().id + "\">" + a.analyses.Count() + "</a>").Named("Análises").Encode(false);
            Column.For(a =>
                TagBuilders.ImageButtonGrid("javascript:callpdetailsfile(false," + a.id + ")", "/Content/ico/details.png", "Detalhes") +

                TagBuilders.ImageButtonGrid("javascript:callpfmaliciousfile(" + a.id + ", true," + ((a.malicious.HasValue && a.malicious.Value)?"true":"false") + 
                    ")", a.malicious.HasValue && a.malicious.Value ? "/Content/ico/malicious.png" : "/Content/ico/malicious_disable.png", "Malicioso") +
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