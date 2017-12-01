using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGPSearch
{
    public class SearchMatch
    {
        public string ProjectFile { get; set; }
        public string ItemLabel { get; set; }
        public string ItemType { get; set; }
        public string ProcessFlow { get; set; }
        public string Location { get; set; }
        public string MatchedLine { get; set; }

        public SearchMatch()
        { }

        public SearchMatch(string label, string itemType, string location, string matchedLine)
        {
            ItemLabel = label;
            ItemType = itemType;
            Location = location;
            MatchedLine = matchedLine;
        }
    }
}
