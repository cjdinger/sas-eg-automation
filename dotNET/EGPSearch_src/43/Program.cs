using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EGPSearch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region Redirect the working path for this process
            string formerDirectory = System.Environment.CurrentDirectory;

            // sometimes during automation, part of the EG project are
            // expanded to the current working directory
            // Redirecting the Current Working Directory will
            // help to keep this stuff out of your way
            string tempPath = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("TEMP"), 
                string.Format("EGPSEARCH{0}", System.Diagnostics.Process.GetCurrentProcess().Id));

            try
            {
                if (!System.IO.Directory.Exists(tempPath))
                    System.IO.Directory.CreateDirectory(tempPath);

                System.Environment.CurrentDirectory = tempPath;
            }
            catch
            {
                // no big deal, just couldn't redirect the temp path
            }

            #endregion

            // "Install" the assembly resolver, so that
            // subsequent calls to EG scripting objects
            // will know where to find the SAS Enterprise Guide
            // application DLLs (assemblies)
            SAS.EG.Automation.SEG43AssemblyResolver.Install();
            Application.Run(new MainSearchWindow("4.3"));

            #region Try to clean up the working directory
            try
            {
                System.Environment.CurrentDirectory = formerDirectory;
                System.IO.Directory.Delete(tempPath, true);
            }
            catch
            {
                // no big deal, we tried to clean up
            }
            #endregion

        }
    }
}
