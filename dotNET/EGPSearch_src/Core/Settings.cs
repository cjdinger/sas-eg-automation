using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace EGPSearch
{
    /// <summary>
    /// A simple "settings" remembering class to help remember preferences from one 
    /// application use to the next
    /// </summary>
    sealed public class AppUserSettings
    {
        #region private/internal functions
        private static string appSettingsLoc = "";
        private static string AppSettingsLoc
        {
            get
            {
                // calculate this location upon first use
                if (appSettingsLoc.Length == 0)
                    appSettingsLoc = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\SAS\\EnterpriseGuide");
                return AppUserSettings.appSettingsLoc;
            }
        }

        internal static Dictionary<string, string> getSettings(string appID)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();

            try
            {
                if (File.Exists(System.IO.Path.Combine(AppSettingsLoc, appID)))
                {
                    try
                    {
                        string content = File.ReadAllText(System.IO.Path.Combine(AppSettingsLoc, appID));
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);
                        XmlNodeList nl = doc.SelectNodes("Settings/add");
                        foreach (XmlNode n in nl)
                        {
                            string key = n.Attributes["key"].Value;
                            string value = n.Attributes["value"].Value;
                            if (!string.IsNullOrEmpty(key) && value != null)
                            {
                                settings.Add(key, value);
                            }
                        }
                    }
                    catch (Exception )
                    {
                        // if there is any exception, we should delete the file.
                        try
                        {
                            File.Delete(System.IO.Path.Combine(AppSettingsLoc, appID));
                        }
                        catch (Exception )
                        {
                        }
                    }
                }
            }
            catch (Exception )
            {
            }

            return settings;
        }

        internal static void storeSettings(string appID, Dictionary<string, string> settings)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement el = doc.CreateElement("Settings");
            foreach (string key in settings.Keys)
            {
                if (key.Length > 0)
                {
                    XmlElement add = doc.CreateElement("add");
                    add.SetAttribute("key", key);
                    add.SetAttribute("value", settings[key]);
                    el.AppendChild(add);
                }
            }
            doc.AppendChild(el);

            // store the information in the file
            // make sure the directory exists
            if (!System.IO.Directory.Exists(AppSettingsLoc))
                System.IO.Directory.CreateDirectory(AppSettingsLoc);

            // create the file with the settings
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(System.IO.Path.Combine(AppSettingsLoc, appID)))
            {
                sw.WriteLine(doc.OuterXml);
                sw.Close();
            }

        }
        #endregion

        #region Main API interface
        /// <summary>
        /// Write a key-value pair to this task-specific settings file
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void WriteValue(string appID, string key, string value)
        {
            try
            {
                Dictionary<string, string> settings = getSettings(appID);

                // set or replace the value for this key
                if (settings.ContainsKey(key))
                    settings[key] = value;
                else settings.Add(key, value);

                storeSettings(appID, settings);
            }
            catch (Exception )
            {
            }
        }

        /// <summary>
        /// Read a key-value pair from the task-specific settings file
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadValue(string appID, string key)
        {
            string value = "";
            Dictionary<string, string> settings = getSettings(appID);
            if (settings.ContainsKey(key))
                value = settings[key];
            return value;
        }

        #endregion
    }

}
