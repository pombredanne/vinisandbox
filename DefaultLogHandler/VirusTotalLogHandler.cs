using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ViniSandbox.Models;
using ViniSandbox.Modules;

namespace ViniSandbox
{
    public class VirusTotalLogHandler : LogHandler
    {
        [MethodParserAttribute]
        public List<antivirus_scan> ParseVirusScan(string logContent)
        {
            List<antivirus_scan> m = new List<antivirus_scan>();
            var ma = Regex.Match(logContent, @".+(?:\n\[\d+\] (?<AV>.+)\r\n\t.+: (?<RES>.+)\r\n\t.+: (?<VER>.+)\r\n\t.+: (?<UP>.+)\r)+");

            var av = ma.Groups["AV"].Captures;
            var res = ma.Groups["RES"].Captures;
            var ver = ma.Groups["VER"].Captures;
            var up = ma.Groups["UP"].Captures;

            for (int i = 0; i < av.Count; i++)
            {
                var aux = new antivirus_scan();
                aux.antivirus = new antivirus() { name = av[i].Value };
                aux.av_last_update = DateTime.Parse(up[i].Value);
                aux.av_version = (ver[i].Value == "-")?"":ver[i].Value;
                aux.result = (res[i].Value == "-")?"":res[i].Value;
                m.Add(aux);
            }

            return m;
        }
    }
}
