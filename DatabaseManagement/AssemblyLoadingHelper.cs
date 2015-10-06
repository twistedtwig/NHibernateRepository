using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DatabaseManagement
{
    /// <summary>
    /// Manages Project assemblies.
    /// </summary>
    internal class AssemblyLoadingHelper
    {
        private static Dictionary<string, Assembly> _libs = new Dictionary<string, Assembly>();
        private static List<string> _hintPaths = new List<string>();

        /// <summary>
        /// Clears cache, (store of assmeblies and hint paths).
        /// </summary>
        internal static void Reset()
        {
            _libs = new Dictionary<string, Assembly>();
            _hintPaths = new List<string>();
        }

        /// <summary>
        /// Hint paths can be added to help find assembly dependencies.  Path can be a folder or DLL.
        /// If its a folder it will find all DLL's in that folder.
        /// </summary>
        /// <param name="path"></param>
        internal static void AddHintPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && !_hintPaths.Contains(path))
            {
                if (Directory.Exists(path))
                {
                    var dlls = Directory.GetFiles(path, "*.dll");
                    foreach (var dll in dlls)
                    {
                        if (!_hintPaths.Contains(dll))
                        {
                            _hintPaths.Add(dll);                            
                        }
                    }
                }
                else
                {
                    _hintPaths.Add(path);                    
                }
            }
        }

        /// <summary>
        /// Event handler method to resolve DLL assemblies that can't be found automatically.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly;
            string keyName = new AssemblyName(args.Name).Name;
            if (keyName.Contains(".resources"))
            {
                return null;  // This line is what fixed the problem
            }

            //Go through hint paths to see if we can help add assembly references
            var referenceFromHintPaths = _hintPaths.Where(p => p.EndsWith(string.Format(@"\{0}.dll", keyName)));
            foreach (var hintPath in referenceFromHintPaths)
            {
                assembly = Assembly.LoadFile(hintPath);
                if (!_libs.ContainsKey(keyName))
                {
                    _libs.Add(keyName, assembly);
                }
            }
            
            if (_libs.ContainsKey(keyName))
            {
                assembly = _libs[keyName]; // If DLL is loaded then don't load it again just return
                return assembly;
            }
            
            string dllName = DllResourceName(keyName);
            //string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();   // Uncomment this line to debug the possible values for dllName
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dllName))
            {
                if (stream == null)
                {
                    Debug.Print("Error! Unable to find '" + dllName + "'");
                    // Uncomment the next lines to show message the moment an assembly is not found. (This will also stop for .Net assemblies
                    //MessageBox.Show("Error! Unable to find '" + dllName + "'! Application will terminate.");
                    //Environment.Exit(0);
                    return null;
                }

                byte[] buffer = new BinaryReader(stream).ReadBytes((int)stream.Length);
                assembly = Assembly.Load(buffer);

                _libs[keyName] = assembly;
                return assembly;
            }
        }

        private static string DllResourceName(string ddlName)
        {
            if (ddlName.Contains(".dll") == false) ddlName += ".dll";

            foreach (string name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (name.EndsWith(ddlName)) return name;
            }
            return ddlName;
        }
    }
}
