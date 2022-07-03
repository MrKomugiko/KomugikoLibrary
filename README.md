# KomugikoLibrary


## example code creation (*.CS)
```cs
  protected ParagrafSectionBuilder mainsection = new ParagrafSectionBuilder();
  protected void Page_Load(object sender, EventArgs e)
  {
      // need excluslivly create paragrad object for a futher refernce link, example below
      Paragraf paragrafReference_1 = new Paragraf();
      Paragraf paragrafReference_2 = new Paragraf();
      Paragraf paragrafReference_3 = new Paragraf();

      mainsection.NewParagraph()
          .AddElement("Element standard auto numerated")
              .AddReferenceParagraf(paragrafReference_1, paragrafReference_2) // add multiple references at onc list
              .AddReferenceParagraf(paragrafReference_3) // add Second reference list
              .AddSubElement("a) Reference to multiple ~REFERENCE~. And Reference to otherParagraf ~REFERENCE~. ")
          .AddReferenceParagraf(paragrafReference_2) // repeat reference
          .AddElement("repeat reference to ~REFERENCE~");

      mainsection.AttachParagraph(paragrafReference_1)
          .SetStyleOfElementRendering(ContentFormatType.td_single_full_width)
          .AddElement("element with no number, full width");

      mainsection.AttachParagraph(paragrafReference_2)
          .AddElement("numbered row")
          .SetStyleOfElementRendering(ContentFormatType.td_single_full_width)
          .AddElement("full width")
          .ResetElementsStyle()
          .AddElement("again back to numbers")
              .AddSubElement("a) sub-element");

      mainsection.AttachParagraph(paragrafReference_3)
          .AddElement("new refernce paragraf 3");
  }
```

## example usage (*.ASPX [ASP.NET])
```aspx
*.ASPX
  <%foreach(var _paragraf in mainsection.ParagrafList){ %>
      <table border="0" cellspacing="0" class="defaultTable">
          <tr>
              <%=_paragraf.GetHeader() %>
          </tr>
          <%foreach (var _element in _paragraf.Content) { %>
              <tr>
                  <%=_element.GetFormattedContent()%>           
              </tr>
          <%} %>
      </table>
      <%}%>
```


## Result
![HTML result](https://github.com/MrKomugiko/KomugikoLibrary/blob/master/paragrafExample.png)


## RenderedOutput
```html
*.HTML
<table border="0" cellspacing="0" class="defaultTable">
  <tbody>
    <tr>
      <td colspan="2" class="paragraph-header">§26</td>
    </tr>
    <tr>
      <td class="paragraph-point">1.</td>
      <td class="paragraph-content">Element standard auto numerated<br>a) Reference to multiple §27,§28. And Reference to otherParagraf §29. </td>           
    </tr>
    <tr>
      <td class="paragraph-point">2.</td>
      <td class="paragraph-content">repeat reference to §28</td>           
    </tr>
  </tbody>
</table>
                                
 <table border="0" cellspacing="0" class="defaultTable">
  <tbody>
    <tr>
      <td colspan="2" class="paragraph-header">§27</td>
    </tr>
    <tr>
      <td colspan="2" class="paragraph-content">element with no number, full width</td>           
    </tr>                              
  </tbody>
 </table>
                                
<table border="0" cellspacing="0" class="defaultTable">
  <tbody>
    <tr>
      <td colspan="2" class="paragraph-header">§28</td>
    </tr>
    <tr>
      <td class="paragraph-point">1.</td>
      <td class="paragraph-content">numbered row</td>           
    </tr>
    <tr>
      <td colspan="2" class="paragraph-content">full width</td>           
    </tr>
    <tr>
      <td class="paragraph-point">2.</td>
      <td class="paragraph-content">again back to numbers<br>a) sub-element</td>           
    </tr>
  </tbody>
</table>

<table border="0" cellspacing="0" class="defaultTable">
  <tbody>
    <tr>
      <td colspan="2" class="paragraph-header">§29</td>
    </tr>
    <tr>
      <td class="paragraph-point">1.</td>
      <td class="paragraph-content">new refernce paragraf 3</td>           
    </tr>
  </tbody>
</table>

```
