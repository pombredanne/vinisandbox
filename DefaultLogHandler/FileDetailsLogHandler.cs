using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViniSandbox.Models;
using System.Text.RegularExpressions;
using ViniSandbox.Modules;
using System.Globalization;

namespace ViniSandbox
{
    public class FileDetailsLogHandler : LogHandler
    {    
        [MethodParser]
        public file_detail ParseFileDetails(string logContent)
        {
            file_detail fd = new file_detail();
            var ma = Regex.Match(logContent, @"MD5: (?<MD5>.+)\r\nSHA1: (?<SHA1>.+)\r\nSHA256: (?<SHA256>.+)\r\nSHA512: (?<SHA512>.+)\r\nCRC32: (?<CRC32>.+)\r\nSSDEEP: (?<SSDEEP>.+)\r\nType: (?<TYPE>.+)\r\nCreation Date: (?<CD>.+)\r\nModification Date: (?<MD>.+)\r\n");            

            fd.md5 = ma.Groups["MD5"].Value;
            fd.sha1 = ma.Groups["SHA1"].Value;
            fd.sha256 = ma.Groups["SHA256"].Value;
            fd.sha512 = ma.Groups["SHA512"].Value;
            fd.crc32 = ma.Groups["CRC32"].Value;
            fd.ssdeep = ma.Groups["SSDEEP"].Value;
            fd.type = ma.Groups["TYPE"].Value;
            fd.create_date = DateTime.ParseExact(ma.Groups["CD"].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            fd.modified_date = DateTime.ParseExact(ma.Groups["MD"].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            return fd;
        }
    }
}
