using iTextSharp.text;

namespace Hyzen.SDK.PdfGenerator.DTO.Pdf;

public record PdfParameters
{
    public int ContentsAlignment { get; set; } = Element.ALIGN_LEFT;
    public int HeaderAlignment { get; set; } = Element.ALIGN_RIGHT;
    public List<string> HeaderList { get; set; } = new();
    public List<PdfContent> ContentList { get; set; } = new();
    public byte[] Logo { get; set; }
    public byte[] SignatureImage{ get; set; }
    public string SignatureWord { get; set; }
    public string TitleDocument { get; set; }
    public string SubTitleDocument { get; set; }
}