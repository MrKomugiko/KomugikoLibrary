using KomugikoLibrary_Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static KomugikoLibrary.ParagrafSectionBuilder;

namespace KomugikoLibrary
{
    public class ParagrafSectionBuilder
    {
        public List<Paragraf> ParagrafList;

        public ParagrafSectionBuilder(List<Paragraf> paragraflist)
        {
            ParagrafList = paragraflist;
        }

        public Paragraf NewParagraph()
        {
            Paragraf _paragraf = new Paragraf(this);
            ParagrafList.Add(_paragraf);
            return _paragraf;
        }
        public Paragraf AttachParagraph(Paragraf _paragraf)
        {
            _paragraf.AttachToParent(this);
            ParagrafList.Add(_paragraf);
            return _paragraf;
        }
    }
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
                    while(ContentList[i].IndexOf(key) > -1 && ParentParagraf.ParagrafReferenceList.Count > referenceIndex)
                    {  
                    int keywordStartIndex = ContentList[i].IndexOf(key);
                        ContentList[i] = ContentList[i].Remove(keywordStartIndex, keywordLength);
                        ContentList[i] = ContentList[i].Insert(keywordStartIndex, 
                            String.Join(",",ParentParagraf.ParagrafReferenceList[referenceIndex++]
                                .Select(x=>x.ReferenceString).ToList()));
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
    public static class ParagrafExtensions
    {
        public static Paragraf AddElement(this Paragraf parent, string element)
        {
            parent.AddContent(new ParagrafElement(element,parent));
            return parent;
        }
        public static Paragraf AddSubElement(this Paragraf parent, string element)
        {
            parent.Content.Last().AddToContent(element);
            return parent;
        }
        /// <summary>
        /// zwyczajne doczepienie kropki do ostatniego wpisu, w przypadku gdy jakis IF stoi nam przed koncem elementu;
        /// </summary>
        public static Paragraf AddDotToLastElement(this Paragraf parent)
        {
            parent.Content.Last().ContentList.Last().Append(Convert.ToChar("."));
            return parent;
        }
        public static Paragraf SetStyleOfElementRendering(this Paragraf parent, ContentFormatType style)
        {
            parent.elementDefaultStyle = style;
            return parent;
        }
        public static Paragraf ResetElementsStyle(this Paragraf parent)
        {
            parent.elementDefaultStyle = ContentFormatType.td_split_numbered;
            return parent;
        }
        public static Paragraf AddReferenceParagraf(this Paragraf parent, params Paragraf[] _references)
        {
            parent.AddReferences(_references);
            return parent;
        }


    }
}
