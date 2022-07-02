using KomugikoLibrary_Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KomugikoLibrary
{
    public class ParagrafElement
    {
        public ContentFormatType elementStyle = ContentFormatType.td_split_numbered;
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
        public string GetContent()
        {
            if(ParentParagraf.hasReference)
            {
                string key = "~REFERENCE~";
                int keywordLength = key.Length;
                int referenceIndex = 0;

                for (int i = 0; i < ContentList.Count; i++)
                {
                    bool ReferenceSlotExist = ContentList[i].IndexOf(key) > -1;
                    bool ReferenceExist = ParentParagraf.ParagrafReferenceList.Count > referenceIndex;

                    while (ReferenceSlotExist && ReferenceExist)
                    {  
                        int keywordStartIndex = ContentList[i].IndexOf(key);
                        string referenceJoinedString = String.Join(",", ParentParagraf.ParagrafReferenceList[referenceIndex++].Select(x => x.ReferenceString).ToList());
                        ContentList[i] = ContentList[i].Remove(keywordStartIndex, keywordLength).Insert(keywordStartIndex, referenceJoinedString);
                    }
                }
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
