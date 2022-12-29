
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Maanfee.Dashboard.Examples.Views.JsInterop.Components;

public partial class DashboardPageSection
{
   protected string Classname =>
        new CssBuilder("mx-4")
            .AddClass($"outlined", /*Outlined &&*/ ChildContent != null )
            .AddClass($"darken", DarkenBackground)
            .AddClass("show-code", _hasCode && ShowCode)
            .AddClass(Class)
            .Build();
    protected string ToolbarClassname =>
        new CssBuilder("docs-section-content-toolbar")
            .AddClass($"outlined", /*Outlined &&*/ ChildContent != null)
            .AddClass("darken", ChildContent == null && Codes != null)
            .Build();

    protected string InnerClassname =>
        new CssBuilder("docs-section-content-inner")
            .AddClass($"relative d-flex flex-grow-1 flex-wrap justify-center align-center", !Block)
            .AddClass($"d-block mx-auto", Block)
            .AddClass($"mud-width-full", Block && FullWidth)
            .AddClass("pa-8", !_hasCode)
            .AddClass("px-8 pb-8 pt-2", _hasCode)
            .Build();
    
    protected string SourceClassname =>
        new CssBuilder("mx-4")
            .AddClass($"outlined", /*Outlined &&*/ ChildContent != null)
            .AddClass("show-code", _hasCode && ShowCode)
            .Build();

	[Parameter] public string Title { get; set; }
	[Parameter] public string Class { get; set; }
    [Parameter] public bool DarkenBackground { get; set; }
    //[Parameter] public bool Outlined { get; set; } = true;
    [Parameter] public bool ShowCode { get; set; } = true;
    [Parameter] public bool Block { get; set; }
    [Parameter] public bool FullWidth { get; set; }
    [Parameter] public string HighLight { get; set; }
    [Parameter] public IEnumerable<CodeFile> Codes { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

	// ***************************************************

	[Parameter] public string ExecutionCode { get; set; }

	[Parameter] public string CodeFileName { get; set; }

	// ***************************************************

	private bool _hasCode;
    private string _activeCode;

    protected override void OnParametersSet()
    {
        if(Codes != null)
        {
            _hasCode = true;
            _activeCode = Codes.FirstOrDefault().code;
        }
        else if(!String.IsNullOrWhiteSpace(CodeFileName))
        {
            _hasCode = true;
            _activeCode = CodeFileName;
        }
    }
    
    public void OnShowCode()
    {
        ShowCode = !ShowCode;
    }

    public void SetActiveCode(string value)
    {
        _activeCode = value;
    }

    private string GetActiveCode(string value)
    {
        if (value == _activeCode)
        {
            return "file-button active";
        }
        else
        {
            return "file-button";
        }
    }

    RenderFragment CodeComponent(string code) => builder =>
    {
        try
        {
            var key = typeof(DashboardPageSection).Assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains($".{code}Code.html"));
            var stream = typeof(DashboardPageSection).Assembly.GetManifestResourceStream(key);
            var reader = new StreamReader(stream);
            {
                var Read = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(HighLight))
                {
                    if (HighLight.Contains(","))
                    {
                        var highlights = HighLight.Split(",");

                        foreach (var value in highlights)
                        {
							Read = Regex.Replace(Read, $"{value}(?=\\s|\")", $"<mark>$&</mark>");
                        }
                    }
                    else
                    {
						Read = Regex.Replace(Read, $"{HighLight}(?=\\s|\")", $"<mark>$&</mark>");
                    }
                }

				//Read = Read.Replace("<", "&lt;");
				//Read = Read.Replace(">", "&gt;");
				//Read = Read.Replace("\"", "&quot;");

				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "class", "html");

				builder.OpenElement(2, "pre");
				builder.AddContent(3, Read);
				builder.CloseElement();

				builder.CloseElement();

				//builder.AddMarkupContent(0, Read);
            }
        }
        catch (Exception)
        {
            // todo: log this
        }
    };
    
}
