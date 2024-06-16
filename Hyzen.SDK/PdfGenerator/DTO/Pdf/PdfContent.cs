namespace Hyzen.SDK.PdfGenerator.DTO.Pdf;

public record PdfContent
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string LinkMessage { get; set; }
    public string LinkUrl { get; set; }
}