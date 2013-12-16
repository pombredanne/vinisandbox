using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace ViniSandbox
{
    public class LogManager
    {
        private static Mutex LogMutex = new Mutex(false, "Vinisandbox-Logfile");
        public enum EVerboseLevel
        { Debug = 3, Normal = 2, Error = 1 }

        public static EVerboseLevel VerboseLevel
        { get; set; }

        public static string LogPath
        { get; set; }

        public static void WriteLine(string line, EVerboseLevel verbose)
        {
            if (verbose <= VerboseLevel)
            {
                Console.WriteLine(line);
                LogMutex.WaitOne();
                try
                {
                    using (var sr = File.AppendText(LogPath))
                    {
                        sr.WriteLine(line);
                    }
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
                LogMutex.ReleaseMutex();
            }
        }
    }
}
