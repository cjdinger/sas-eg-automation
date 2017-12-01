using System;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace SAS.EG.Automation
{
    /// <summary>
    /// Class to help you use SAS Enterprise Guide classes from their installed location
    /// </summary>
    public class SEG43AssemblyResolver
    {
        #region internal members
        // these are the default install paths for our 4.3/9.2 products
        internal static string PathToEGuideInstall = @"C:\Program Files\SAS\EnterpriseGuide\4.3";
        #endregion

        /// <summary>
        /// Install the AssemblyResolver event listener and discover locations of installed assemblies
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when unable to locate the SAS Enterprise Guide 4.3 installed location.</exception>
        public static void Install()
        {
            // initialize EG path

            RegistryKey regKey = null;
            try
            {
                // determine EG 4.3 location using InstallShield key
                using (regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\SAS Institute Inc.\Enterprise Guide\4.3", false))
                {
                    if ((regKey != null) && (regKey.GetValue("InstallLocation") != null))
                    {
                        string path = regKey.GetValue("InstallLocation") as string;
                        if (path.Length > 0)
                            PathToEGuideInstall = path;
                    }
                    else
                        throw new System.IO.FileNotFoundException("Cannot locate SAS Enterprise Guide 4.3.  Is SAS Enterprise Guide 4.3 installed?");
                }
            }
            catch
            {
                throw new System.IO.FileNotFoundException("Cannot locate SAS Enterprise Guide 4.3.  Is SAS Enterprise Guide 4.3 installed?");
            }

            // install Assembly Resolver event
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Resolve assemblies not found in the current directory
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="args">contains a Name property with the assembly name needed</param>
        /// <returns>Loaded assembly, loaded by this routine</returns>
        private static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resolvepath;
            // all files that are "early bound" should be in the Enterprise Guide directory
            resolvepath = PathToEGuideInstall;

            string[] name = args.Name.Split(',');
            string path = System.IO.Path.Combine(resolvepath, name[0] + ".dll");
            try
            {
                Assembly foundAssembly = Assembly.LoadFile(path);
                return foundAssembly;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new System.IO.FileNotFoundException("Could not load assembly from expected location", path, ex);
            }
        }
    }
}
