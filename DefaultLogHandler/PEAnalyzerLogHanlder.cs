using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ViniSandbox.Models;
using ViniSandbox.Modules;
            
namespace ViniSandbox
{
    public class PEAnalyzerLogHanlder : LogHandler
    {
        [MethodParser]
        public pe_file ParsePeFile(string logContent)
        {
            pe_file m = new pe_file();
            var ma = Regex.Match(logContent, @"Meta-data\r\n=+\r\n(?:(?<TIP>.+): (?<VAL>.*)\r\n)+");
            var values = ma.Groups["VAL"].Captures;
            if (values.Count > 9)
            {
                m.architecture = values[2].Value;
                var ma2 = values[6].Value;
                try
                {
                    ma2 = ma2.Split('[')[1].Split(']')[0];
                    m.compilation_date = DateTime.ParseExact(ma2, "ddd MMM dd HH:mm:ss yyyy UTC", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                }
                m.language = values[7].Value;
                if (ma.Groups["TIP"].Captures[9].Value.Contains("Entry Point"))
                    m.entry_point = values[9].Value;
                else
                {
                    m.packer = values[9].Value;
                    m.entry_point = values[10].Value;
                }

                return m;   
            }
            throw new LogParserException();
        }
        
        [MethodParser]
        public List<section> ParseSection(string logContent)
        {
            List<section> sections = new List<section>();
            var ma = Regex.Match(logContent, @"Sections\r\n=+.\n.+\n-+\r\n(?<SECTION>.+\r\n)+");
            foreach (var section in ma.Groups["SECTION"].Captures)
            {
                section sec = new section();
                var val = Regex.Match(section.ToString(), @"((?<VAL>[^\s]+)\s*)+");
                var captures = val.Groups["VAL"].Captures;
                if(captures.Count > 5)
                {
                    sec.name = captures[0].Value;
                    sec.virtual_address = captures[1].Value;
                    sec.virtual_size = captures[2].Value;
                    sec.raw_size = captures[3].Value;
                    sec.md5 = captures[4].Value;                                   
                }
                sec.suspicious = captures.Count == 7;
                sections.Add(sec);
            }
            return sections;
        }
        
        [MethodParser]
        public List<resource> ParseResource(string logContent)
        {
            List<resource> resources = new List<resource>();
            //var ma = Regex.Match(logContent, @"Resource entries\n=+\n.+\n-+\n(?<RES>.+\n)+");
            var ma = Regex.Match(logContent, @"Name.+RVA.+Size.+Lang.+Sublang.+Type\r\n-+\r\n(?<RES>.+\r\n)+");
            foreach (var resource in ma.Groups["RES"].Captures)
            {
                //var val = Regex.Match(resource.ToString(), @"((?<VAL>[^\s]+)\s+)+");
                var val = Regex.Match(resource.ToString(), @"((?<VAL>[^\s]+)\s+){1,5}(?<UVAL>.+)\r");
                var captures = val.Groups["VAL"].Captures;
                if (captures.Count == 5)
                {
                    resource res = new resource();
                    res.name = captures[0].Value;
                    res.size = captures[2].Value;
                    res.language = captures[3].Value;
                    if(val.Groups["UVAL"].Success)
                        res.resource_type = new resource_type() { name = val.Groups["UVAL"].Value };
                    //res.type = captures[5].Value;
                    resources.Add(res);
                }
            }
            return resources;
        }
        
        [MethodParser]
        public List<import_library> ParseImport(string logContent)
        {
            List<import_library> imports = new List<import_library>();
            //var ma = Regex.Match(logContent, @"Imports\n=+\n(?<IMP>\[\d\] (?<LIB>.+)\n(?<FUN>[^\[,^\n]+\n)+)+");
            //var ma = Regex.Match(logContent, @"Imports\n=+\n(?<IMP>\[\d\] (?<LIB>.+)\n(?:\t(?<FUN>[^\[,^\n]+)\n)+)+");
            var ma = Regex.Match(logContent, @"Imports\r\n=+\r\n(?:\[\d+\] (?<LIB>.+)\r\n(?:\t(?<FUN>[^\[,^\r]+)\r\n)+)+");
            int FunIdx = 0;
            var libs = ma.Groups["LIB"].Captures;
            var funcs = ma.Groups["FUN"].Captures;
            for (int i = 0; i < libs.Count; i++)
			{
			    import_library lib = new import_library();
                lib.name = libs[i].Value;
                for (;FunIdx < funcs.Count && ((i+1 < libs.Count && libs[i+1].Index > funcs[FunIdx].Index) || libs.Count == i+1); FunIdx++)
			    {
			        import_function fun = new import_function();
                    var func_splited = funcs[FunIdx].Value.Split(new char[] { ' ' });
                    if (func_splited.Length == 2)
                    {
                        import_function function = new import_function();
                        function.offset = func_splited[0];
                        function.name = func_splited[1];
                        lib.import_function.Add(function);
                    }
			    }
                imports.Add(lib);
			}
            return imports;
        }
        
        [MethodParser]
        public List<miscellaneous> ParseMiscellaneous(string logContent)
        {
            List<miscellaneous> miscs = new List<miscellaneous>();

            var ma = Regex.Match(logContent, @"Adobe Malware Classifier.+\n(?:.\[\+\] (?<VAL>.+)\r\n)+");
            foreach (Capture item2 in ma.Groups["VAL"].Captures)
            {
                miscellaneous m = new miscellaneous();
                m.type = "Adobe Malware Classifier";
                m.description = item2.Value;
                miscs.Add(m);
            }
            ma = Regex.Match(logContent, @"Anomalies/Flags.+\n(?:.\[\+\] (?<VAL>.+)\r\n)+");
            foreach (Capture item2 in ma.Groups["VAL"].Captures)
            {
                miscellaneous m = new miscellaneous();
                m.type = "Anomalies/Flags";
                m.description = item2.Value;
                miscs.Add(m);
            }
            ma = Regex.Match(logContent, @"Anti-VM.+\n(?:.\[\+\] (?<VAL>.+)\r\n)+");
            foreach (Capture item2 in ma.Groups["VAL"].Captures)
            {
                miscellaneous m = new miscellaneous();
                m.type = "Anti-VM";
                m.description = item2.Value;
                miscs.Add(m);
            }
            ma = Regex.Match(logContent, @"Anti-Dbg.+\n(?:.\[\+\] (?<VAL>.+)\r\n)+");
            foreach (Capture item2 in ma.Groups["VAL"].Captures)
            {
                miscellaneous m = new miscellaneous();
                m.type = "Anti-Dbg";
                m.description = item2.Value;
                miscs.Add(m);
            }
            ma = Regex.Match(logContent, @"Embedded File\(s\).+\n(?:.\[\+\] (?<VAL>.+)\r\n)+");
            foreach (Capture item2 in ma.Groups["VAL"].Captures)
            {
                miscellaneous m = new miscellaneous();
                m.type = "Embedded File";
                m.description = item2.Value;
                miscs.Add(m);
            }
            ma = Regex.Match(logContent, @"URLs.+\n(?:.\[\+\] (?<VAL>.+)\r\n)+");
            foreach (Capture item2 in ma.Groups["VAL"].Captures)
            {
                miscellaneous m = new miscellaneous();
                m.type = "URLs";
                m.description = item2.Value;
                miscs.Add(m);
            }
            return miscs;
        }
    }
}
