using System;
using System.Linq;
using System.Reflection;

namespace ViniSandbox.Modules
{
    public static class ModuleLoader<T>
    {
        public static T LoadModule(string AssemblyName, string ClassName, params object[] parameters)
        {
            Assembly asm = null;
            Type cl;
            Type tType = typeof(T);

            try
            {
                asm = Assembly.LoadFile(Environment.CurrentDirectory + "\\" + AssemblyName);
            }
            catch (Exception)
            {
                throw new Exception(AssemblyName + " cannot be loaded");
            }

            try
            {
                cl = asm.GetType(ClassName);
            }
            catch (Exception)
            {
                throw new Exception(ClassName + " cannot be founded");
            }

            if ((tType.IsAbstract && cl.BaseType == tType) || (tType.IsInterface && cl.GetInterfaces().Contains(tType)))
            {
                return (T)Activator.CreateInstance(cl, parameters);
            }            

            throw new Exception(ClassName + " is not a " + tType.Name);
        }
    }
}
