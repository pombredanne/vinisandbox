using System;
using System.Linq;
using System.Reflection;
using System.IO;

namespace ViniSandbox.Modules
{
    public delegate void ObjectExtracted(object obj);

    public abstract class LogHandler
    {
        public void ParseLog(string LogPath, ObjectExtracted objExtracted)
        {
            bool find = false;
            string logContent = "";
            using (var sw = new StreamReader(LogPath))
            {
                logContent = sw.ReadToEnd();
            }

            foreach (MethodInfo methods in GetType().GetMethods())
            {
                var att = methods.GetCustomAttributes(typeof(MethodParserAttribute), true);                
                if(att.Count() != 0)
                {
                    var parameters = methods.GetParameters();
                    if (parameters.Count() == 1 && parameters.ElementAt<ParameterInfo>(0).ParameterType == typeof(string))
                    {
                        find = true;
                        try
                        {
                            object ret = methods.Invoke(this, new object[] { logContent });
                            if (ret != null && objExtracted != null)
                                objExtracted(ret);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format("Exception throwed at {0} on Log {1}", methods.Name, LogPath));
                        }
                    }
                }
            }

            if(!find)
                throw new Exception("Cannot be found a MethodParser to on " + GetType().FullName);
        }
    }
}
