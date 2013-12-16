using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebViniSandbox.Models.ViewModel;
using ViniSandbox.Models;

namespace WebViniSandbox.Mapper
{
    public class Mapper
    {
        public List<Info> HashMapper(file_detail det)
        {
            List<Info> hs = new List<Info>();
            hs.Add(new Info() { Nome = "CRC32", Valor = det.crc32 });
            hs.Add(new Info() { Nome = "MD5", Valor = det.md5 });
            hs.Add(new Info() { Nome = "SHA1", Valor = det.sha1 });
            hs.Add(new Info() { Nome = "SHA256", Valor = det.sha256 });
            hs.Add(new Info() { Nome = "SHA512", Valor = det.sha512 });
            hs.Add(new Info() { Nome = "SSDEEP", Valor = det.ssdeep });

            hs.Add(new Info() { Nome = "Tipo", Valor = det.type });
            if (det.pe_file != null)
            {
                hs.Add(new Info() { Nome = "Arquitetura", Valor = det.pe_file.architecture });
                hs.Add(new Info() { Nome = "Data de Compilação", Valor = det.pe_file.compilation_date.HasValue?det.pe_file.compilation_date.Value.ToString():"" });
                hs.Add(new Info() { Nome = "Língua", Valor = det.pe_file.language });
                hs.Add(new Info() { Nome = "Packer", Valor = det.pe_file.packer });
                hs.Add(new Info() { Nome = "Entry Point", Valor = det.pe_file.entry_point });
            }
            return hs;
        }
    }
}