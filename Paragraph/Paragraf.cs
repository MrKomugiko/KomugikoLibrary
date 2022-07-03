using KomugikoLibrary_Client.Enums;
using System;
using System.Collections.Generic;

namespace KomugikoLibrary
{
    public class Paragraf
    {
        public List<Paragraf[]> ParagrafReferenceList = new List<Paragraf[]>();
        public List<ParagrafElement> Content = new List<ParagrafElement>();

        public const string ParagraphSign = "§";
        public string StyleClass_header = "paragraph-header";

        public bool hasReference = false;
        public int NextElementNumber = 1;
        public int NextReferenceIndexToLoad = 0;
        public ContentFormatType elementDefaultStyle;
       
        public int Number
        {
            get
            {
                if (parentSection == null) return -1;

                int elementIndex = parentSection.ParagrafList.FindIndex(x => x == this);
                return elementIndex + 1; // number start counting from 1
            }
        }
        public string ReferenceString 
        {
            get
            {
                if (Number > -1)
                    return String.Format("{0}{1}", ParagraphSign, Number);
                else
                    return "[REFERENCE NOT FOUND]";
            }
        }

        private ParagrafSectionBuilder parentSection;
        internal bool RecentElementAddingStatus;

        #region constructors
        public Paragraf() {

        }
        public Paragraf(ParagrafSectionBuilder _parent) {
            parentSection = _parent;
        }

        #endregion

        public void AddReferences(Paragraf[] _references)
        {
            hasReference = true;
            ParagrafReferenceList.Add(_references);
        }
        public void AttachToParent(ParagrafSectionBuilder paragrafSectionBuilder)
        {
            parentSection = paragrafSectionBuilder;
        }
        public void AddContent(params ParagrafElement[] contentList)
        {
            foreach(var element in contentList)
            {
                element.elementStyle = elementDefaultStyle;
            }
            Content.AddRange(contentList);
        }
        public string GetHeader()
        {
            return String.Format("<td colspan='2' class='{0}'>", StyleClass_header) +
                    String.Format("{0}{1}", ParagraphSign,Number) +
                    "</td>";
        }
    }
}
