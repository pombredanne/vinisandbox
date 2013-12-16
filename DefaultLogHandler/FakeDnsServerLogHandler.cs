using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViniSandbox.Modules;
using ViniSandbox.Models;
using System.Text.RegularExpressions;

namespace ViniSandbox
{
    public class FakeDnsServerLogHandler : LogHandler
    {
        [MethodParser]
        public List<dns> ParseFakeDnsServer(string logContent)
        {
            List<dns> dns = new List<Models.dns>();
            var ma = Regex.Match(logContent, @"((?<DOMAIN>.+)\r\n)+");

            foreach (var domain in ma.Groups["DOMAIN"].Captures)
            {
                dns.Add(new dns() { domain = domain.ToString() });
            }
            return dns;
        }
    }
}
