using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EGPSearch
{
    public class Wildcard
    {
        static public bool containsWildcards(string source)
        {
            string fileTo = getFilename(source, true);
            if (fileTo.LastIndexOfAny(new char[] { '*', '?' }) > -1)
                return true;
            else
                return false;
        }

        static public string getFilename(string source, bool withExt)
        {
            string filename;
            int pos = source.LastIndexOf(@"\");
            if (pos == -1)
                pos = source.LastIndexOf("/");
            filename = source.Substring(pos + 1);

            if (!withExt)
            {
                int extPos = filename.LastIndexOf(".");
                int fileLen = filename.Length;
                filename = filename.Substring(1, fileLen - extPos - 1);
            }
            return filename;
        }

        static public string getFolder(string source)
        {
            string folder;
            int pos = source.LastIndexOf(@"\");
            if (pos == -1)
                pos = source.LastIndexOf("/");
            folder = source.Substring(0, pos + 1);
            return folder;
        }
    }
    
}

// Using extension methods to add a new behavior for DirectoryInfo
namespace Extensions
{
    public static class CopyExtensions
    {
        /// <summary>
        /// Get the files that match a three-character extension (such as *.xls) without
        /// matching further characters in the name (such as *.xlsx).
        /// Found this snippet on CodeProject:
        /// http://www.codeproject.com/Articles/153471/DirectoryInfo-GetFiles-returns-more-files-than-exp
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static FileInfo[] GetFilesByExactMatchExtension(
         this DirectoryInfo directory, string searchPattern, SearchOption searchOption)
        {
            FileInfo[] result = directory.GetFiles(searchPattern, searchOption);
            if (0 != result.Length)
            {
                string extension = Path.GetExtension(searchPattern);
                if (null != extension && 4 == extension.Length)
                {
                    var matchingFiles = from fileInfo in result
                                        where string.Equals(extension, fileInfo.Extension, StringComparison.OrdinalIgnoreCase)
                                        select fileInfo;

                    result = matchingFiles.ToArray();
                }
            }

            return result;
        }

        public static FileInfo[] GetFilesByExactMatchExtension(this DirectoryInfo directory, string searchPattern)
        {
            return GetFilesByExactMatchExtension(directory, searchPattern, SearchOption.TopDirectoryOnly);
        }
    }
}