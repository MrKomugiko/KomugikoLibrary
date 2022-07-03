using KomugikoLibrary_Client.Enums;
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
        public static Paragraf AddSubElement(this Paragraf parent, string element)
        {
            parent.Content.Last().AddToContent(element);
            return parent;
        }
        public static Paragraf AddNumeratedSubElement(this Paragraf parent, string element)
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
