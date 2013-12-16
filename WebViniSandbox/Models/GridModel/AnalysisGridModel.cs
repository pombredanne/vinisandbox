using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViniSandbox.Models;
using MvcContrib.UI.Grid;
using WebViniSandbox.Helpers;

namespace WebViniSandbox.Models
{
    public class AnalysisGridModel : GridModel<analysis>
    {
        public AnalysisGridModel()
        {                     
            Column.For(a => a.start_date);
            Column.For(a => a.final_date);
            Column.For(a => a.antivirus_scan.Count(p => !string.IsNullOrEmpty(p.result)) + "/" + a.antivirus_scan.Count()).Named("Antivírus Scan");
            Column.For(a => a.computer_event.Count()).Named("Eventos");
            Column.For(a => a.dns.Count()).Named("Dominios");
            Column.For(a => a.file_detail.pe_file.packer).Named("Packer");
            Column.For(a =>
                TagBuilders.ImageLinkGrid("/Analysis/Details?id=" + a.id, "/Content/ico/details.png", "Detalhes") +

                TagBuilders.ImageButtonGrid("javascript:callpfmaliciousfile(" + a.file_detail.id + ", false," + ((a.file_detail.malicious.HasValue && a.file_detail.malicious.Value)?"true":"false") + 
                    ")", a.file_detail.malicious.HasValue && a.file_detail.malicious.Value ? "/Content/ico/malicious.png" : "/Content/ico/malicious_disable.png", "Malicioso") +
                TagBuilders.ImageButtonGrid("javascript:callpfsendantivirusfile(" + a.file_detail.id + ",false)", "/Content/ico/send.png", "Enviar Amostra") +
                TagBuilders.ImageButtonGrid("javascript:callpfreanalizefile(" + a.file_detail.id + ",false)", "/Content/ico/redo.png", "Reanalisar")
                //enviar empresa de antivirus
                //malicioso                
                //reanalisar
                //Numero de analises
            ).Encode(false).Sortable(false).Named("Ações");
        }
    }
}