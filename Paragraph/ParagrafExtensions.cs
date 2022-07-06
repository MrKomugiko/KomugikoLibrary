using KomugikoLibrary.Enums;
using System;
using System.Linq;

namespace KomugikoLibrary
{
    public static class ParagrafExtensions
    {
        public static Paragraf AddElement(this Paragraf parent, ParagrafElement element)
        {
            element.SetParent(parent);
            parent.AddContent(element);
            return parent;
        }
        public static Paragraf AddElement(this Paragraf parent, string element)
        {
            parent.AddContent(new ParagrafElement(element,parent));
            return parent;
        }

        public static Paragraf ConditionallyAddElement(this Paragraf parent, bool result, string element)
        {
            if(result == false)
            {
                parent.RecentElementAddingStatus = false;
            }
            else
            {
                parent.AddContent(new ParagrafElement(element, parent));
            }

            return parent;
        }

        public static Paragraf AddSubElement(this Paragraf parent, string element)
        {
            parent.Content.Last().AddToContent(element);
            return parent;
        }
        public static Paragraf ConditionallySubElement(this Paragraf parent, bool result, string element)
        {
            if (result == true)
            {
                parent.Content.Last().AddToContent(element);
            }

            return parent;
        }
        public static Paragraf AddSubElementRequireParent(this Paragraf parent, string element)
        {
            // check if last adding Paragraf was added succesfully
            if(parent.RecentElementAddingStatus == true)
            {
                parent.Content.Last().AddToContent(element);
            }
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

        public static Paragraf SetGlobalStyleClassForElements(this Paragraf parent, string all_classStyle)
        {
            parent.Global_StyleClass_content = all_classStyle;
            parent.Global_StyleClass_point = all_classStyle;
            return parent;
        }
        public static Paragraf SetGlobalStyleClassForElementsPoint(this Paragraf parent,string point_classStyle)
        {
            parent.Global_StyleClass_point = point_classStyle;
            return parent;
        }
        public static Paragraf SetGlobalStyleClassForElementsContent(this Paragraf parent,string content_classStyle)
        {
            parent.Global_StyleClass_content = content_classStyle;
            return parent;
        }
        public static Paragraf ResetGlobalStyleClassesForElements(this Paragraf parent)
        {
            parent.Global_StyleClass_content = null;
            parent.Global_StyleClass_point = null;
            return parent;
        }
        public struct ConditionResultData
        {
            public bool Result;
            public string ElementContent;
        }
    }
}
