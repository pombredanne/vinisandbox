using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using ViniSandbox.Modules;
using ViniSandbox.Models;
using System.Globalization;

namespace ViniSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(@"C:\Users\Vinicius\Documents\Visual Studio 2010\Projects\ViniSandbox\ViniSandbox\bin\Debug\Log\FileDetails.txt"))
            {
                string a = sr.ReadToEnd();

                LogHandler l = new PEAnalyzerLogHanlder();
                l.ParseLog(@"C:\Users\Vinicius\Documents\Visual Studio 2010\Projects\ViniSandbox\ViniSandbox\bin\Debug\Log\AnalyzePe.txt", null);
            }
            //using (StreamReader sr = new StreamReader(@"C:\Users\Vinicius\Downloads\AnalyzePE-master\AnalyzePE-master\output_examples\verbose.txt"))
            //{
            //    string a = sr.ReadToEnd();
                
            //    LogHandler l = new PEAnalyzerLogHanlder();

            //    var metadata = l.ParseLog<metadata>(a);
            //    var imports = l.ParseLog<List<import_library>>(a);
            //    var misc = l.ParseLog<List<miscellaneous>>(a);
            //    var resource = l.ParseLog<List<resource>>(a);
            //    var section = l.ParseLog<List<section>>(a);
            //}

            //using (StreamReader sr = new StreamReader(@"C:\downloads\Analise\Dinamica Basica\ProcessMonitor\Logfile.XML"))
            //{
            //    string a = sr.ReadToEnd();
            //    LogHandler l = new ProcessMonitorLogHandler();

            //    var events = l.ParseLog<List<computer_event>>(a);
            //}

            //using (StreamReader sr = new StreamReader(@"C:\Users\Vinicius\Desktop\avteste.txt"))
            //{
            //    string a = sr.ReadToEnd();

            //    LogHandler l = new VirusTotalLogHandler();
                
            //    var sdd = l.ParseLog<List<antivirus_scan>>(a);
            //}

            
        }
    }
}
