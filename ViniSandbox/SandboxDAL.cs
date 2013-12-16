using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViniSandbox.Models;
namespace ViniSandbox
{
    public class SandboxDAL
    {        
        private analysis analysis = null;
        private List<computer_event> computer_events = new List<computer_event>();
        private List<antivirus_scan> antivirus_scans = new List<antivirus_scan>();
        private List<dns> dns_list = new List<dns>();
        private List<miscellaneous> miscellaneous = new List<miscellaneous>();
        private List<resource> resources = new List<resource>();
        private file_detail file_detail = null;
        private pe_file pe_file = null;
        private List<export_function> export_functions = new List<export_function>();
        private List<import_function> import_functions = new List<import_function>();
        private List<section> sections = new List<section>();
        private List<result_file> result_file = new List<result_file>();
        private List<import_library> import_libraries = new List<import_library>();

        public void Clean()
        {
            analysis = null;
            computer_events.Clear();
            antivirus_scans.Clear();
            dns_list.Clear();
            miscellaneous.Clear();
            resources.Clear();
            file_detail = null;
            pe_file = new pe_file();
            export_functions.Clear();
            import_functions.Clear();
            sections.Clear();
            result_file.Clear();
            import_libraries.Clear();
        }

        public void addObject(object obj)
        {
            if (obj is analysis)
                analysis = (analysis)obj;
            else if (obj is List<computer_event>)
                computer_events.AddRange((List<computer_event>)obj);
            else if (obj is List<antivirus_scan>)
                antivirus_scans.AddRange((List<antivirus_scan>)obj);
            else if (obj is List<dns>)
                dns_list.AddRange((List<dns>)obj);
            else if (obj is List<miscellaneous>)
                miscellaneous.AddRange((List<miscellaneous>)obj);
            else if (obj is List<resource>)
                resources.AddRange((List<resource>)obj);
            else if (obj is file_detail)
                file_detail = (file_detail)obj;
            else if (obj is pe_file)
                pe_file = (pe_file)obj;
            else if (obj is List<export_function>)
                export_functions.AddRange((List<export_function>)obj);
            else if (obj is List<import_function>)
                import_functions.AddRange((List<import_function>)obj);
            else if (obj is List<section>)
                sections.AddRange((List<section>)obj);
            else if (obj is result_file)
                result_file.Add((result_file)obj);
            else if(obj is List<import_library>)
                import_libraries.AddRange((List<import_library>)obj);
        }

        public void Save(file_detail file_det)
        {
            vinisandboxContext cx = new vinisandboxContext();

            file_det = cx.file_detail.Find(file_det.id);            

            foreach (var anti_scan in antivirus_scans)
            {
                var antivirus = anti_scan.antivirus;
                var bdV = cx.antivirus.ToArray().FirstOrDefault(p => p.Equals(antivirus));
                if (bdV != null)
                {
                    anti_scan.antivirus = bdV;
                    bdV.antivirus_scan.Add(anti_scan);
                }
                analysis.antivirus_scan.Add(anti_scan);
            }

            foreach (var comp_event in computer_events)
            {
                analysis.computer_event.Add(comp_event);
            }

            foreach (var dns in dns_list)
            {
                var domain = dns;
                var bdDns = cx.dns.ToArray().FirstOrDefault(p => p.Equals(dns));
                if (bdDns != null)
                    domain = bdDns;
                domain.analyses.Add(analysis);
                analysis.dns.Add(domain);             
            }

            foreach (var re_file in result_file)
            {
                analysis.result_file.Add(re_file);
            }

            foreach (var misc in miscellaneous)
            {
                analysis.miscellaneous.Add(misc);
            }

            file_det.analyses.Add(analysis);


            foreach (var res in resources)
            {
                var bdRes = cx.resource_type.ToArray().FirstOrDefault(p => p.Equals(res.resource_type));
                if (bdRes != null)
                {
                    res.resource_type = bdRes;
                    bdRes.resources.Add(res);
                }
                pe_file.resources.Add(res);
            }

            foreach (var sec in sections)
            {
                pe_file.sections.Add(sec);
            }

            foreach (var exp_func in export_functions)
            {
                pe_file.export_function.Add(exp_func);
            }

            foreach (var imp_lib in import_libraries)
            {
                var imp_lib_rec = imp_lib;
                var bdIl = cx.import_library.ToArray().FirstOrDefault(p => p.Equals(imp_lib));
                if (bdIl != null)
                {
                    imp_lib_rec = bdIl;
                }

                foreach (var imp_func in imp_lib.import_function)
                {
                    imp_func.import_library = imp_lib_rec;
                    var imp_func_rec = imp_func;
                    var bdIf = cx.import_function.ToArray().FirstOrDefault(p => p.Equals(imp_func));
                    if (bdIf != null)
                    {
                        imp_func_rec = bdIf;
                    }
                    else
                        imp_func_rec.import_library = imp_lib_rec;                   
                    pe_file.import_function.Add(imp_func_rec);
                }
            }

            pe_file aux2 = cx.pe_file.SingleOrDefault(p => p.id == file_det.id);
            if (aux2 != null)
            {
                var remRes = aux2.resources.ToList();
                for (int i = 0; i < remRes.Count; i++)                
                    cx.resources.Remove(remRes[i]); 
                aux2.resources.Clear();                    

                var remSec = aux2.sections.ToList();                
                for (int i = 0; i < remSec.Count; i++)                    
                    cx.sections.Remove(remSec[i]);                    
                aux2.sections.Clear();

                var remExp = aux2.export_function.ToList();
                for (int i = 0; i < remExp.Count; i++)                
                    cx.export_function.Remove(remExp[i]);
                aux2.export_function.Clear();

                var remImp = aux2.import_function;
                aux2.import_function.Clear();
                cx.pe_file.Remove(aux2);
                cx.SaveChanges();
            }

            file_det.pe_file = pe_file;
            pe_file.file_detail = file_det;

            file_det.type = file_detail.type;
            file_det.md5 = file_detail.md5;
            file_det.sha1 = file_detail.sha1;
            file_det.sha256 = file_detail.sha256;
            file_det.sha512 = file_detail.sha512;
            file_det.crc32 = file_detail.crc32;
            file_det.ssdeep = file_detail.ssdeep;
            file_det.modified_date = file_detail.modified_date;
            file_det.create_date = file_detail.create_date;

            cx.SaveChanges();
        }
    }
}
