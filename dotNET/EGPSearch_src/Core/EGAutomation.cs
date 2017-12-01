using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAS.EG.Automation
{
    /// <summary>
    /// Helper class to make certain automation tasks easier with SAS Enterprise Guide
    /// </summary>
    public static class EGAutomation
    {
        /// <summary>
        /// The EG application object
        /// </summary>
        public static SAS.EG.Scripting.Application EGApp
        { get; private set; }

        /// <summary>
        /// The EG Project object
        /// </summary>
        public static SAS.EG.Scripting.ISASEGProject EGProject
        { get; private set; }

        /// <summary>
        /// static constructor
        /// </summary>
        static EGAutomation() { EGApp = null; }

        /// <summary>
        /// Start EG
        /// </summary>
        /// <param name="profileName"></param>
        public static void StartEG(string profileName)
        {
            // end any existing App instance, just in case
            EndEG();

            EGApp = new SAS.EG.Scripting.Application();
            if (!string.IsNullOrEmpty(profileName))
                EGApp.SetActiveProfile(profileName);
        }

        /// <summary>
        /// Start EG
        /// </summary>
        public static void StartEG() 
        {
            StartEG(string.Empty);
        }

        /// <summary>
        /// Close active project and end EG session
        /// </summary>
        public static void EndEG()
        {
            if (EGProject != null)
            {
                try
                {
                    EGProject.Close();
                }
                catch
                { }
                EGProject = null;
            }

            if (EGApp != null)
            {
                try
                {
                    EGApp.Quit();
                }
                catch
                { }
                EGApp = null;
            }
        }

        /// <summary>
        /// Open a project file
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="password"></param>
        public static void OpenProject(string projectPath, string password)
        {
            EGProject = EGApp.Open(projectPath, password);
        }

        /// <summary>
        /// Get the text of a note from the active project
        /// </summary>
        /// <param name="processFlow">Name of the process flow that contains the note</param>
        /// <param name="noteLabel">Name of the note object (exact spelling)</param>
        /// <returns></returns>
        public static string GetNoteText(string processFlow, string noteLabel)
        {
            if (EGAutomation.EGProject != null && !string.IsNullOrEmpty(processFlow))
            {
                foreach (SAS.EG.Scripting.ISASEGContainer container in EGAutomation.EGProject.ContainerCollection)
                {
                    if (container.ContainerType == 0 && 
                        string.Compare(container.Name.Trim(),processFlow.Trim(),true) == 0)
                    {
                        try
                        {
                            if (container.Notes[noteLabel] != null)
                                return ((SAS.EG.Scripting.ISASEGNote)container.Notes[noteLabel]).Text;
                        }
                        catch { }
                        break;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Run an ordered list in the active project
        /// </summary>
        /// <param name="listName">Name of the Ordered List to run</param>
        public static void RunOrderedList(string listName)
        {
            // Run the Ordered List that represents the work to do
            SAS.EG.Scripting.ISASEGOrderedList ol = FindOrderedList(listName);
            if (ol != null)
            {
                try
                {
                    ol.Run();
                }
                catch { }
            }
        }

        /// <summary>
        /// Helper function to find a particular ordered list
        /// </summary>
        /// <param name="orderedListName">name of the ordered list in the project</param>
        /// <returns>the ordered list interface, if found</returns>
        /// <exception cref="System.Exception">Thrown if no ordered list by that name is found</exception>
        internal static SAS.EG.Scripting.ISASEGOrderedList FindOrderedList(string orderedListName)
        {
            SAS.EG.Scripting.ISASEGOrderedList ol = null;
            foreach (SAS.EG.Scripting.ISASEGContainer container in EGProject.ContainerCollection)
            {
                // filter out just the OrderedLists container
                if (container.ContainerType == 5)
                {
                    try
                    {
                        ol = container.Items[orderedListName] as SAS.EG.Scripting.ISASEGOrderedList;
                    }
                    catch 
                    {
                        throw new Exception(string.Format("Ordered List {0} not found in project", orderedListName));
                    }
                    break;
                }
            }
            return ol;
        }

        /// <summary>
        /// Run a process flow, in its entirety
        /// </summary>
        /// <param name="processFlow">Name of the process flow to run</param>
        /// <returns></returns>
        public static bool RunProcessFlow(string processFlow)
        {
            if (EGAutomation.EGProject != null)
            {
                foreach (SAS.EG.Scripting.ISASEGContainer container in EGAutomation.EGProject.ContainerCollection)
                {
                    if (container.ContainerType == 0 && (string.Compare(container.Name.Trim(),processFlow.Trim(),true) == 0))
                    {
                        container.Run();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Run a SAS program and return the log
        /// </summary>
        /// <param name="codeToRun">The text of a SAS program</param>
        /// <returns>The log as a result</returns>
        public static string RunProgram(string codeToRun)
        {
            if (codeToRun.Length > 0)
            {
                SAS.EG.Scripting.ISASEGCode code = EGAutomation.EGProject.CodeCollection.Add();
                code.Text = codeToRun;
                code.Run();
                return code.Log.Text;
            }
            return string.Empty;
        }


        /// <summary>
        /// Retrieve all log output from the items in an ordered list
        /// </summary>
        /// <param name="processFlow">Name of the ordered list in the project</param>
        /// <returns>a concatenated string with all log content contained in items in the ordered list</returns>
        public static string RetrieveOrderedListLogs(string orderedList)
        {
            StringBuilder sb = new StringBuilder();

            SAS.EG.Scripting.ISASEGOrderedList ol = FindOrderedList(orderedList);
            if (ol != null)
            {
                foreach (SAS.EG.Scripting.ISASEGItem item in ol.Items)
                {
                    sb.Append(GetItemLog(item));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retrieve all log output from a process flow
        /// </summary>
        /// <param name="processFlow">Name of the process flow in the project</param>
        /// <returns>a concatenated string with all log content contained in the flow</returns>
        public static string RetrieveProcessFlowLogs(string processFlow)
        {
            StringBuilder sb = new StringBuilder();

            if (EGAutomation.EGProject != null)
            {
                foreach (SAS.EG.Scripting.ISASEGContainer container in EGAutomation.EGProject.ContainerCollection)
                {
                    if (container.ContainerType == 0 && container.Name == processFlow)
                    {
                        foreach (SAS.EG.Scripting.ISASEGItem item in container.Items)
                        {
                            sb.Append(GetItemLog(item));
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the attached log output for any particular item
        /// </summary>
        /// <param name="item">Handle to the item</param>
        /// <returns>The log text, if found</returns>
        private static string GetItemLog(SAS.EG.Scripting.ISASEGItem item)
        {
            switch (item.Type)
            {
                case (int)SAS.EG.Scripting.SASEGItemType.egLog:
                    SAS.EG.Scripting.ISASEGLog log = item as SAS.EG.Scripting.ISASEGLog;
                    return log.Text;
                case (int)SAS.EG.Scripting.SASEGItemType.egCode:
                    SAS.EG.Scripting.ISASEGCode code = item as SAS.EG.Scripting.ISASEGCode;
                    if (code.Log != null && code.Log.Text != null)
                        return code.Log.Text;
                    break;
                case (int)SAS.EG.Scripting.SASEGItemType.egTask:
                    SAS.EG.Scripting.ISASEGTask task = item as SAS.EG.Scripting.ISASEGTask;
                    if (task.Log != null && task.Log.Text != null)
                        return task.Log.Text;
                    break;
                case (int)SAS.EG.Scripting.SASEGItemType.egQuery:
                    SAS.EG.Scripting.ISASEGQuery query = item as SAS.EG.Scripting.ISASEGQuery;
                    if (query.Log != null && query.Log.Text != null)
                        return query.Log.Text;
                    break;
                default:
                    return string.Empty;
            }

            return string.Empty;
        }
    }
}
