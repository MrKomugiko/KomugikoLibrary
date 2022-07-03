using KomugikoLibrary_Client.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KomugikoLibrary
{
    public class ParagrafElement
    {
        public List<string> ContentList = new List<string>();
        
        public ContentFormatType elementStyle;
        public string StyleClass_point = "paragraph-point";
        public string StyleClass_content = "paragraph-content";

        private Paragraf ParentParagraf;
        
        /// <summary>
        /// get element number and incerase it for next element
        /// </summary>
        private int Number
        {
            get
            {
                int number = ParentParagraf.NextElementNumber;
                ParentParagraf.NextElementNumber++;
                return number;
            }
        }

        #region constructors
        public ParagrafElement(string _content) {
            ContentList.Add(_content);
        }
        public ParagrafElement(string _content, Paragraf _parent=null)
        {
            if(_parent != null)
            {
                ParentParagraf = _parent;
            }
            ContentList.Add(_content);
            
            _parent.RecentElementAddingStatus = true;
        }
        #endregion
        
        /// <summary>
        /// Add given string (allow HTML) to exisitng content list of this element.
        /// </summary>
        /// <param name="_content">single string, html allowed </param>
        public void AddToContent(string _content)
        {
            ContentList.Add(_content);
        }
        
        /// <summary>
        /// private flag indicationg if references string was loaded into element content list
        /// </summary>
        private bool ReferenceLoaded = false;

        /// <summary>
        /// Replacing special keyword <c>~REFERENCE~</c> from content elements with associated links to referenced paragrafs <br />
        /// get references from parent-Paragraf references array, and replace every <c>~REFERENCE~</c> one by one relative to reference array><br />
        /// Possible to add multiple references to single reference array , then reference string will be joined separated by ','<br />
        /// <br />
        /// <example> Example:<br />
        ///     list trough content list looking for key: "element is referencing to ~REFERENCE~ paragraph."<br />
        ///     <br />
        ///     updated element when 1 reference: <br /><c>"element is referencing to §5 paragraph."</c><br />
        ///     <br />
        ///     updated element when multiple reference:<br /><c>"element is referencing to §5,§2,§3 paragraph."</c><br />
        ///      <br />
        ///     updated element when missing reference:<br /><c>"element is referencing to §5,[REFERENCE NOT FOUND] paragraph."</c>
        /// </example>
        /// </summary>
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
        
        /// <summary>
        /// First load paragraf references if exist, then join all content of this Element separated with html new line.
        /// </summary>
        public string GetContent()
        {
            if(ParentParagraf.hasReference && ReferenceLoaded==false)
            {
                LoadReferences();
            }
            
            return String.Join("<br />", ContentList);
        }

        ///<summary>
        /// full formatted html string with full element content 
        /// </summary>
        /// <returns>
        /// If element style is: td_split_numbered: <br />
        /// * <c>return 2x td elements one with element number and second with its content</c> <br />
        ///  <br />
        /// If element style is: td_single_full_width <br />
        /// * <c>return only one td with content on full width</c>
        /// </returns>
        public string GetFormattedContent()
        {
            switch(elementStyle)
            {
                default:
                case ContentFormatType.td_split_numbered:
                    return String.Format("<td class='{0}'>{1}.</td>", StyleClass_point, Number) +
                            String.Format("<td class='{0}'>{1}</td>", StyleClass_content,GetContent());
                       
                case ContentFormatType.td_single_full_width:
                    return String.Format("<td colspan='2' class='{0}'>{1}</td>",StyleClass_content, GetContent());
            }
        }
        
        public void SetParent(Paragraf _parent)
        {
            ParentParagraf = _parent;
        }
    }
}
