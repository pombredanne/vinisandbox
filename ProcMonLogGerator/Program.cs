using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Threading;

namespace ProcMonLogGerator
{
    static class Program
    {
        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string procmon = AssemblyDirectory + "\\procmon.exe";
                Process.Start(procmon, "/Terminate").WaitForExit();
                Process.Start(procmon, string.Format("/Openlog {0} /SaveAs {1}", args[0], args[1])).WaitForExit();             
            }
            catch (Exception ex)
            {
                File.WriteAllText(Environment.GetEnvironmentVariable("temp") + "\\" + "logg.txt", ex.Message + "\n\n" + ex.StackTrace);
                MessageBox.Show(ex.ToString());
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
