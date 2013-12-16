using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Rug.Cmd;

namespace FileDetails
{
    class Program
    {
        private static StreamWriter sw = null;

        private static string hashToString(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private static void printResult(string res)
        {
            Console.WriteLine(res);
            if (sw != null)
                sw.WriteLine(res);
        }

        static int Main(string[] args)
        {
            ArgumentParser parser = new ArgumentParser("FileDetails", "This show some details of a file");
            StringArgument outPath = new StringArgument("out", "output file", "output file to store result");
            StringArgument filePath = new StringArgument("f", "File", "input file");
            parser.Add("\\", "out", outPath);
            parser.Add("\\", "file", filePath);

            if (parser.HelpMode)
            {
                parser.WriteLongArgumentsUsage();
                return 0;
            }

            try
            {
                parser.Parse(args);

                string file = "";
                if(!filePath.Defined)
                    throw new Exception();
                file = filePath.Value;

                if (outPath.Defined)
                    sw = new StreamWriter(outPath.Value);

                FileInfo fi = new FileInfo(file);
                byte[] bfile = new byte[fi.Length];
                using (var sr = fi.OpenRead())
                {
                    sr.Read(bfile, 0, bfile.Length);
                }

                MD5 m = MD5.Create();
                byte[] rm = m.ComputeHash(bfile);
                printResult("MD5: " + hashToString(rm));

                SHA1 s1 = SHA1.Create();
                byte[] rs1 = s1.ComputeHash(bfile);
                printResult("SHA1: " + hashToString(rs1));

                SHA256 s2 = SHA256.Create();
                byte[] rs2 = s2.ComputeHash(bfile);
                printResult("SHA256: " + hashToString(rs2));

                SHA512 s5 = SHA512.Create();
                byte[] rs5 = s5.ComputeHash(bfile);
                printResult("SHA512: " + hashToString(rs5));

                Crc32 c = new Crc32();
                byte[] crc32 = c.ComputeHash(bfile);
                printResult("CRC32: " + hashToString(crc32));

                StringBuilder ssdeep = new StringBuilder(100);
                ssdeepWrapper.fuzzy_hash_filename(file, ssdeep);
                printResult("SSDEEP: " + ssdeep);

                var a = new FileInfo(file);
                byte[] data = new byte[a.Length];
                using (var aux = fi.OpenRead())
                {
                    aux.Read(data, 0, data.Length);
                }

                ProcessStartInfo si = new ProcessStartInfo("file.exe", "\"" + file + "\"");
                si.RedirectStandardOutput = true;
                si.UseShellExecute = false;
                si.WindowStyle = ProcessWindowStyle.Hidden;

                var proc = Process.Start(si);
                string saida = proc.StandardOutput.ReadToEnd();
                saida = Regex.Match(saida, ".+; (?<tipo>.+)\r").Groups["tipo"].Value;
                printResult("Type: " + saida);

                printResult("Creation Date: " + fi.CreationTime.ToString("dd/MM/yyyy HH:mm:ss"));
                printResult("Modification Date: " + fi.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss"));

                if (sw != null)
                    sw.Close();
            }
            catch (Exception)
            {
                parser.WriteShortArgumentsUsage();
                return -1;
            }
            return 0;
        }
    }
}
