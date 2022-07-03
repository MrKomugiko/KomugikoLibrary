using KomugikoLibrary_Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KomugikoLibrary
{
    public class ParagrafElement
    {
        public ContentFormatType elementStyle;
        private readonly Paragraf ParentParagraf;
        public int GetNumber()
        {
            int number = ParentParagraf.NextElementNumber;
            ParentParagraf.NextElementNumber++;
            return number;
        }
        public List<string> ContentList = new List<string>();
        public ParagrafElement(string _content, Paragraf _parent=null)
        {
            if(_parent != null)
            {
                ParentParagraf = _parent;
            }
            ContentList.Add(_content);
        }
        public void AddToContent(string _content)
        {
            ContentList.Add(_content);
        }
        
        private bool ReferenceLoaded = false;
        private void LoadReferences()
        {
            string key = "~REFERENCE~";
            int keywordLength = key.Length;
            int referenceIndex = ParentParagraf.NextReferenceIndexToLoad;

            for (int elementIndex = 0; elementIndex < ContentList.Count; elementIndex++)
            {
                if (ParentParagraf.ParagrafReferenceList.Count - 1 < referenceIndex) break;

                while (ContentList[elementIndex].IndexOf(key) > 1)
                {
                    int startIndex = ContentList[elementIndex].IndexOf(key);
                    string referenceJoinedString = String.Join(",", ParentParagraf.ParagrafReferenceList[referenceIndex++].Select(x => x.ReferenceString).ToList());
                    ContentList[elementIndex] = ContentList[elementIndex].Remove(startIndex, keywordLength).Insert(startIndex, referenceJoinedString);
                    ParentParagraf.NextReferenceIndexToLoad++;
                }
            }
            ReferenceLoaded = true;
        }
        public string GetContent()
        {
            if(ParentParagraf.hasReference && ReferenceLoaded==false)
            {
                LoadReferences();
            }
            
            return String.Join("<br />", ContentList);
        }

        public string GetFormattedContent()
        {
            switch(elementStyle)
            {
                default:
                case ContentFormatType.td_split_numbered:
                    return String.Format("<td class='paragraph-point'>{0}.</td>", GetNumber()) +
                            String.Format("<td class='paragraph-content'>{0}</td>", GetContent());
                       
                case ContentFormatType.td_single_full_width:
                    return String.Format("<td colspan='2' class='paragraph-content'>{0}</td>", GetContent());
            }
        }
    }
}
