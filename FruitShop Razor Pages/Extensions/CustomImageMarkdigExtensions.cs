using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace FruitShop_Razor_Pages.Extensions;

public class CustomImageMarkdigExtensions : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is not HtmlRenderer htmlRenderer) return;
        var linkRenderer = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();

        linkRenderer?.TryWriters.Insert(0, TryRenderCustomImage);
    }

    private static bool TryRenderCustomImage(HtmlRenderer renderer, LinkInline link)
    {
        if (!link.IsImage)
        {
            return false;
        }

        renderer.Write(
            "<figure style=\"display: flex; flex-direction: column; align-items: center; margin: 1.5rem auto;\">");
        renderer.Write("<img style=\"max-width: 80%; max-height: 500px; object-fit: contain;\" src=\"")
            .WriteEscapeUrl(link.GetDynamicUrl != null ? link.GetDynamicUrl() : link.Url)
            .Write("\"");
        renderer.Write(" alt=\"");
        var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
        renderer.EnableHtmlForInline = false;
        renderer.WriteChildren(link);
        renderer.EnableHtmlForInline = wasEnableHtmlForInline;
        renderer.Write("\"");
        if (!string.IsNullOrEmpty(link.Title))
        {
            renderer.Write(" title=\"").WriteEscape(link.Title).Write("\"");
        }

        if (renderer.EnableHtmlForInline)
        {
            renderer.WriteAttributes(link);
        }

        renderer.Write(" loading=\"lazy\" />");
        renderer.Write(
            "<figcaption style=\"text-align: center; font-style: italic; margin-top: 8px; font-size: 0.9em;\">");
        renderer.WriteChildren(link);
        renderer.Write("</figcaption>");
        renderer.Write("</figure>");

        return true;
    }
}