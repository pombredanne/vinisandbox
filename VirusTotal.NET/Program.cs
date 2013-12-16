using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirusTotalNET.Objects;
using System.IO;
using System.Reflection;
using Rug.Cmd;

namespace VirusTotalNET
{
    public class Program
    {
        public static int Main(string[] vargs)
        {
            int timeout = 648000;

            ArgumentParser parser = new ArgumentParser("VirusTotal.NET", "This application perform a scan of a file on virustotal.com");
            StringArgument outPath = new StringArgument("out", "output file", "output file to store scan result");
            StringArgument filePath = new StringArgument("f", "File", "File to be scanned");
            parser.Add("\\", "out", outPath);
            parser.Add("\\", "file", filePath);

            if (parser.HelpMode)
            {
                parser.WriteLongArgumentsUsage();
                return 0;
            }

            try 
	        {
                parser.Parse(vargs);
                if (!filePath.Defined)
                    throw new Exception();
                try
                {
                    string outFile = "";
                    if (outPath.Defined)
                        outFile = outPath.Value;
                    var report = VirusTotalScanner.Scan(filePath.Value, timeout);
                    printResult(report, outFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return 0;
	        }
	        catch (Exception e)
	        {
                parser.WriteShortArgumentsUsage();
                return -1;
	        }
        }

        private static void printResult(Report r, string file)
        {            
            int c = 1;
            StreamWriter sw = null;
            if(!String.IsNullOrEmpty(file))
                sw = new StreamWriter(file);

            Console.WriteLine(r.ScanDate);
            if (sw != null)
                sw.WriteLine(r.ScanDate);
            foreach (var item in r.Scans)
            {
                string aux = String.Format("[{0}] {1}\r\n\tResult: {2}\r\n\tVersion: {3}\r\n\tLast Update: {4}", c++, item.Name, string.IsNullOrWhiteSpace(item.Result) ? "-" : item.Result, string.IsNullOrWhiteSpace(item.Version) ? "-" : item.Version, item.UpdateString);
                Console.WriteLine(aux);
                if (sw != null)
                    sw.WriteLine(aux);
            }

            if (sw != null)
                sw.Close();
        }
    }
}
