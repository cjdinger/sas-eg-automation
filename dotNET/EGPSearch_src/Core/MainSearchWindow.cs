using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

// for Extension methods of DirectoryInfo
using Extensions;
using System.Text.RegularExpressions;

namespace EGPSearch
{
    public partial class MainSearchWindow : Form
    {

        const string constRecentFilespec = "RecentFilespec";
        const string appID = "EGPSearch";

        #region Pinvoke items for the "help text" in text boxes
        // necessary constants for the 'filter' cue display
        // that is shown in the filter text box
        internal const int EM_SETCUEBANNER = 0x1501;
        internal const int EM_GETCUEBANNER = 0x1502;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern Int32 SendMessage(IntPtr hWnd, int msg,
                int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        #endregion


        public MainSearchWindow(string version)
        {
            InitializeComponent();

            this.Text = string.Format("{0} (v{1})", this.Text, version);

            // initialize the text fields
            SendMessage(txtFilespec.Handle, EM_SETCUEBANNER, 0, "File(s) to search");
            SendMessage(txtSearchFor.Handle, EM_SETCUEBANNER, 0, "Search for");
            txtFilespec.Text = AppUserSettings.ReadValue(appID, constRecentFilespec);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;
            // remember this value for the next instance of this application
            AppUserSettings.WriteValue(appID, constRecentFilespec,txtFilespec.Text);

            lvMatches.Items.Clear();
            statusLabel.Text = string.Format("Searching projects: {0}", txtFilespec.Text);
            txtMessages.AppendText(string.Format("Searching \"{0}\" for \"{1}\"\r\n", txtFilespec.Text, txtSearchFor.Text));

            List<string> egpFiles = MatchFilesToSearch();
            List<SearchMatch> matches = new List<SearchMatch>();
            int matchCount = 0;
            // process each matching file in turn
            foreach (string filename in egpFiles)
            {
                if (File.Exists(filename))
                {
                    txtMessages.AppendText(string.Format("  Opening {0}\r\n", filename));
                    matches = SearchProject(filename, txtSearchFor.Text);
                    foreach (SearchMatch m in matches)
                    {
                        ListViewItem item = ListItemFromMatch(filename, m);
                        lvMatches.Items.Add(item);
                    }
                } 
                matchCount += matches.Count;
            }

            statusLabel.Text = string.Format("{0} project files processed, found {1} matches", egpFiles.Count,matchCount);
            txtMessages.AppendText(string.Format("{0} project files processed, found {1} matches\r\n", egpFiles.Count, matchCount));
        }

        /// <summary>
        /// Create a list view item from one of our "match" objects
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static ListViewItem ListItemFromMatch(string filename, SearchMatch m)
        {
            string file = Path.GetFileName(filename);
            string folder = Path.GetDirectoryName(filename);
            ListViewItem item = new ListViewItem
                (new string[] { m.ItemLabel, m.ItemType, m.ProcessFlow, m.Location, file, folder, m.MatchedLine });
            item.Tag = m;
            return item;
        }

        /// <summary>
        /// Make sure that there are values in the search fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(txtFilespec.Text.Trim()))
            {
                MessageBox.Show("You must specify a file or wildcard pattern to search", "Specify files");
                return false;
            }
            if (string.IsNullOrEmpty(txtSearchFor.Text.Trim()))
            {
                MessageBox.Show("You must specify keyword or phrase to search for", "Specify search string");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Search a project file for occurences of the specified string
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private List<SearchMatch> SearchProject(string filename, string searchString)
        {
            List<SearchMatch> matches = new List<SearchMatch>();
            try
            {
                SAS.EG.Automation.EGAutomation.StartEG();
                SAS.EG.Automation.EGAutomation.OpenProject(filename, "");
                foreach (SAS.EG.Scripting.Container c in SAS.EG.Automation.EGAutomation.EGProject.ContainerCollection)
                {
                    if (c.ContainerType == 0)
                    {
                        foreach (SAS.EG.Scripting.ISASEGItem item in c.Items)
                        {
                            // grab just the last part of the type
                            string type = item.GetType().ToString();
                            string[] tokens = type.Split(new char[] { '.' });
                            if (tokens.Length > 0)
                                type = tokens[tokens.Length - 1];

                            if (matchName(item.Name, searchString))
                            {
                                SearchMatch match = new SearchMatch()
                                {
                                    ProjectFile = filename,
                                    ItemLabel = item.Name,
                                    ProcessFlow = c.Name,
                                    Location = "Label",
                                    ItemType = type
                                };
                                match.MatchedLine = item.Name;

                                matches.Add(match);
                            }

                            matches.AddRange(matchContent(item, c, filename, searchString));
                        }

                        // for handling Note objects, which come in a different collection
                        foreach (SAS.EG.Scripting.ISASEGNote item in c.Notes)
                        {
                            if (matchName(item.Name, searchString))
                            {
                                SearchMatch match = new SearchMatch()
                                {
                                    ProjectFile = filename,
                                    ItemLabel = item.Name,
                                    ProcessFlow = c.Name,
                                    Location = "Label",
                                    ItemType = "Note"
                                };
                                match.MatchedLine = item.Name;

                                matches.Add(match);
                            }

                            matches.AddRange(matchContent(item, c, filename, searchString));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // capture just the first line of a long message
                string[] parts = Regex.Split(ex.Message, "\r\n");
                string message = parts[0];
                txtMessages.AppendText(string.Format("Error searching {0}: {1}\r\n", filename, message));
            }
            finally
            {
                SAS.EG.Automation.EGAutomation.EndEG();
            }

            return matches;
        }

        // Find occurences of the search string in a block of text, such as a program,
        // log file or note
        private List<SearchMatch> matchTextContent(
            string text,
            string searchString,
            string filename,
            string processFlowName,
            string type,
            string label            
            )
        {
            List<SearchMatch> matches = new List<SearchMatch>();
            Regex ex = new Regex(searchString.Trim(), RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string[] lines = Regex.Split(text, "\r\n");
            int lineNumber = 0;
            foreach (string line in lines)
            {
                lineNumber++;
                if (ex.Match(line).Captures.Count > 0)
                {
                    SearchMatch match = new SearchMatch()
                    {
                        ProjectFile = filename,
                        ItemLabel = label,
                        ProcessFlow = processFlowName,
                        Location = string.Format("Line {0}", lineNumber),
                        ItemType = type
                    };
                    match.MatchedLine = line;
                    matches.Add(match);
                }
            }

            return matches;
        }

        // Search an item within the project.  Different actions
        // needed depending on the type of item.
        private List<SearchMatch> matchContent(SAS.EG.Scripting.ISASEGItem item, 
            SAS.EG.Scripting.Container c,
            string filename,
            string searchString)
        {
            List<SearchMatch> matches = new List<SearchMatch>();

            // handle SAS programs that are embedded or easily accessed in the local file system
            if (item is SAS.EG.Scripting.Code)
            {
                matches.AddRange(matchTextContent(
                    ((SAS.EG.Scripting.Code)item).Text,
                    searchString,
                    filename,
                    c.Name,
                    "SAS program",
                    item.Name));

                if (((SAS.EG.Scripting.Code)item).Log != null)
                {
                    matches.AddRange(
                        (matchTextContent(
                    ((SAS.EG.Scripting.Code)item).Log.Text,
                    searchString,
                    filename,
                    c.Name,
                    "SAS log",
                    string.Format("{0} - Log",item.Name))));
                }
            }

            // handle notes
            if (item is SAS.EG.Scripting.Note)
            {
                matches.AddRange(matchTextContent(
                    ((SAS.EG.Scripting.Note)item).Text,
                    searchString,
                    filename,
                    c.Name,
                    "Note",
                    item.Name));
            }

            // handle generated task code and logs
            if (item is SAS.EG.Scripting.Task)
            {
                SAS.EG.Scripting.TaskCode tc = ((SAS.EG.Scripting.Task)item).TaskCode as SAS.EG.Scripting.TaskCode;
                if (tc!=null)
                    matches.AddRange(matchTextContent(
                        tc.Text,
                        searchString,
                        filename,
                        c.Name,
                        "Generated SAS program",
                        string.Format("{0} - Code", item.Name)));

                SAS.EG.Scripting.Log tl = ((SAS.EG.Scripting.Task)item).Log as SAS.EG.Scripting.Log;
                if (tl != null)
                {
                    matches.AddRange(matchTextContent(
                        tl.Text,
                        searchString,
                        filename,
                        c.Name,
                        "Log",
                        string.Format("{0} - Log", item.Name)));
                }
            }

            // handle generated query code and logs
            if (item is SAS.EG.Scripting.Query)
            {
                SAS.EG.Scripting.TaskCode tc = ((SAS.EG.Scripting.Query)item).TaskCode as SAS.EG.Scripting.TaskCode;
                if (tc != null)
                    matches.AddRange(matchTextContent(
                        tc.Text,
                        searchString,
                        filename,
                        c.Name,
                        "Generated SAS program",
                        string.Format("{0} - Code", item.Name)));

                SAS.EG.Scripting.Log tl = ((SAS.EG.Scripting.Query)item).Log as SAS.EG.Scripting.Log;
                if (tl != null)
                {
                    matches.AddRange(matchTextContent(
                        tl.Text,
                        searchString,
                        filename,
                        c.Name,
                        "Log",
                        string.Format("{0} - Log", item.Name)));
                }
            }

            // Data items can have tasks built off of them
            if (item is SAS.EG.Scripting.Data)
            {
                // caution: recursive call!
                foreach (SAS.EG.Scripting.ISASEGItem subitem in ((SAS.EG.Scripting.ISASEGData)item).Tasks)
                    matches.AddRange(matchContent(subitem, c, filename, searchString));
            }


            return matches;
        }

        // for matching just a simple text field, such as a label within the project
        private bool matchName(string name, string searchString)
        {
            Regex ex = new Regex(searchString, RegexOptions.IgnoreCase);
            if (ex.Match(name).Captures.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determine the list of candidate files to search based on the wildcards
        /// </summary>
        /// <returns></returns>
        private List<string> MatchFilesToSearch()
        {
            string source = txtFilespec.Text;
            List<string> matchingFiles = new List<string>();

            if (Wildcard.containsWildcards(source))
            {
                string fileSpec = Path.GetFileName(source);
                DirectoryInfo di = new DirectoryInfo(Wildcard.getFolder(source));
                FileInfo[] files = di.GetFilesByExactMatchExtension(fileSpec, SearchOption.TopDirectoryOnly);
                if (files != null && files.Length > 0)
                {
                    foreach (FileInfo fn in files)
                    {
                        matchingFiles.Add(fn.FullName);
                    }
                }
            }
            else
            {
                matchingFiles.Add(source);
            }

            return matchingFiles;
        }

        // Show About box
        private void aboutProjectSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDlg dlg = new AboutDlg("SAS Enterprise Guide Project File Search");
            dlg.ShowDialog(this);
        }

        // Initiate Save Results
        private void saveSearchResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Text files (*.txt) | *.txt";
            dlg.OverwritePrompt = true;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string outFile = dlg.FileName;
                SaveResults(outFile);
            }
        }

        // Save the results to a text file
        private void SaveResults(string outFile)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Searched {0} for text matching \"{1}\"\r\n", txtFilespec.Text, txtSearchFor.Text);
            sb.AppendFormat("Total matches found: {0}\r\n", lvMatches.Items.Count);
            sb.AppendLine("-------------------------------------");
            foreach (ListViewItem item in lvMatches.Items)
            {
                SearchMatch m = item.Tag as SearchMatch;
                if (m != null)
                {
                    sb.AppendFormat("{0} (process flow: {1}): {2} ({3}) - {4} \"{5}\" \r\n",
                        m.ProjectFile, m.ProcessFlow, m.ItemLabel, m.ItemType, m.Location, m.MatchedLine );
                }
            }

            try
            {
                System.IO.File.WriteAllText(outFile, sb.ToString());
                MessageBox.Show(string.Format("Results saved to {0}", outFile));
            }
            catch
            {
                MessageBox.Show(string.Format("Unable to save results to {0}", outFile));
            }
        }

        private void selectProjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SAS Enterprise Guide project files (*.egp) | *.egp";
            dlg.Multiselect = false;
            if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                txtFilespec.Text = dlg.FileName;
            }
        }

        // make the context menu "dynamic" with the project file name
        private void onContextMenuOpened(object sender, EventArgs e)
        {
            ListViewItem item = null;

            if (lvMatches.SelectedItems.Count > 0)
                item = lvMatches.SelectedItems[0];

            if (item != null && item.Tag !=null)
            {
                openProjectFile.Enabled = true;
                SearchMatch m = item.Tag as SearchMatch;

                openProjectFile.Text = string.Format("Open \"{0}\" in SAS Enterprise Guide", m.ProjectFile);
                openProjectFile.Tag = m.ProjectFile;
            }
            else
            {
                openProjectFile.Enabled = false;
                openProjectFile.Tag = null;
            }
        }

        // open the selected project file in EG
        private void onOpenProjectFile(object sender, EventArgs e)
        {
            string command =
                Path.Combine(SAS.EG.Automation.SEG43AssemblyResolver.PathToEGuideInstall, "SEGuide.exe");

            string file;

            ToolStripItem menu = sender as ToolStripItem;
            if (menu!=null)
            {
                file=menu.Tag as string;
                System.Diagnostics.Process.Start(command, file);
            }
        }


    }
}
