﻿using System.Collections.Generic;
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
}
