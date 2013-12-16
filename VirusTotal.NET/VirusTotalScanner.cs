using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusTotalNET;
using VirusTotalNET.Objects;
using System.Threading;

namespace VirusTotalNET
{
    public class VirusTotalScanner
    {
        public static Report Scan(string file, int timeout)
        {
            int time = 0;
            Report rep = null;

            VirusTotal v = new VirusTotal("2f26e8512174ab034b2df8dc1a58b56c773102a7aa61c2fd78179b0ae8e9647b");
            v.UseTLS = true;

            ScanResult result = v.ScanFile(file);
            
            //nao tem na base de dados
            if (result.ResponseCode == 0)
            {
                
            }
            //esta na lista de analise
            else if (result.ResponseCode == -2)
            {

            }
            //ja tem na base de dados
            else if (result.ResponseCode == 1)
            {
                rep = v.GetFileReport(result.Resource)[0];
                if (rep.ScanDate.Substring(0, 10) != DateTime.Today.ToString("yyyy-MM-dd"))
                {
                    int error = 0;

                    //tenta mandaaar 3 vezes se der erro
                    while(error < 3)
                    {
                        result = v.Rescan(result.Resource)[0];
                        if (result.ResponseCode != 0)
                            break;
                        error++;
                    }

                    if (error == 3)
                        throw new Exception("Error on rescan file SHA256:" + result.Sha256);
                }
            }
            
            while (time <= timeout)
            {                
                rep = v.GetFileReport(result.Resource)[0];
                //ja terminou scan
                if (rep.ResponseCode == 1)
                {
                    return rep;
                }
                time += 60000;
            }
            Thread.Sleep(60000);
            throw new Exception("Timeout on get scan report");
        }
    }
}
