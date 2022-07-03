using KomugikoLibrary_Client.Enums;
using System;
using System.Collections.Generic;

namespace KomugikoLibrary
{
    public class Paragraf
    {
        public ContentFormatType elementDefaultStyle = ContentFormatType.td_split_numbered;

        public const string ParagraphSign = "§";

        #region Reference Settings
        public List<Paragraf[]> ParagrafReferenceList = new List<Paragraf[]>();
        public bool hasReference = false;
        public void AddReferences(Paragraf[] _references)
        {
            hasReference = true;
            ParagrafReferenceList.Add(_references);
        }
        #endregion


        private ParagrafSectionBuilder parentSection;
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
                return String.Format("{0}{1}", ParagraphSign, Number);
            }
        }

        public int NextElementNumber = 1;
        public int NextReferenceIndexToLoad = 0;

        public List<ParagrafElement> Content = new List<ParagrafElement>();
        public Paragraf() {

        }
        public Paragraf(ParagrafSectionBuilder _parent) {
            parentSection = _parent;
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
            return "<td colspan='2' class='paragraph-header'>"+
                    String.Format("{0}{1}", ParagraphSign,Number) +
                    "</td>";
        }
    }
}
