using Hyzen.SDK.PdfGenerator.DTO.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Hyzen.SDK.PdfGenerator;

public class PdfCreator
{
    private static readonly Font FontBold = new(Font.HELVETICA, 12, Font.BOLD);
    private static readonly Font FontBoldPlus = new(Font.HELVETICA, 16, Font.BOLD);
    private static readonly Font Italic = new(Font.HELVETICA, 12, Font.ITALIC);
    private static readonly Font Underline = new(Font.HELVETICA, 12, Font.UNDERLINE);

    public static async Task<byte[]> Create(PdfParameters paramsPdf)
    {
        using MemoryStream stream = new MemoryStream();
        using Document pdf = new Document();

        PdfWriter.GetInstance(pdf, stream);
        pdf.Open();

        PdfPTable header = await CreateHeader(paramsPdf);
        pdf.Add(header);

        Paragraph div = await CreateDivider(75);
        pdf.Add(div);

        Paragraph title = await CreateTitleAndSubtitleDocument(paramsPdf);
        pdf.Add(title);

        Paragraph report = await CreateContent(paramsPdf.ContentList, paramsPdf.ContentsAlignment);
        pdf.Add(report);

        if (paramsPdf.SignatureImage is { Length: > 0 } && !string.IsNullOrEmpty(paramsPdf.SignatureWord))
        {
            Paragraph signature = await CreateSignature(paramsPdf);
            pdf.Add(signature);
        }

        pdf.Close();
        return stream.ToArray();
    }

    private static Task<PdfPTable> CreateHeader(PdfParameters paramsPdf)
    {
        PdfPTable headerTable = new PdfPTable(3);
        float[] columnsWidth = { 1f, 0.15f, 1.5f };
        headerTable.SetWidths(columnsWidth);

        PdfPCell columnLogo = new PdfPCell
        {
            HorizontalAlignment = Element.ALIGN_CENTER,
            VerticalAlignment = Element.ALIGN_MIDDLE,
            Border = Rectangle.NO_BORDER
        };

        if (paramsPdf.Logo != null && paramsPdf.Logo.Length > 0)
        {
            Image imageLogoPdf = Image.GetInstance(paramsPdf.Logo);
            imageLogoPdf.ScaleToFit(150, 150);
            columnLogo.AddElement(imageLogoPdf);
        }

        headerTable.AddCell(columnLogo);
        PdfPCell emptyColumn = new PdfPCell { Border = Rectangle.NO_BORDER };
        headerTable.AddCell(emptyColumn);

        Paragraph headerInfos = new Paragraph();

        if (paramsPdf.HeaderList != null)
            foreach (var headerString in paramsPdf.HeaderList)
                headerInfos.Add(new Paragraph($"{headerString}"));

        PdfPCell informationTable = new PdfPCell(headerInfos)
        {
            HorizontalAlignment = paramsPdf.HeaderAlignment,
            VerticalAlignment = Element.ALIGN_TOP,
            Border = Rectangle.NO_BORDER
        };
        
        headerTable.AddCell(informationTable);

        return Task.FromResult(headerTable);
    }

    private static Task<Paragraph> CreateTitleAndSubtitleDocument(PdfParameters paramsPdf)
    {
        Paragraph titleAndSubtitle = new Paragraph("\n") { Alignment = Element.ALIGN_CENTER };
        titleAndSubtitle.Add(new Phrase($"{paramsPdf.TitleDocument}\n", FontBoldPlus));
        titleAndSubtitle.Add(new Phrase($"{paramsPdf.SubTitleDocument}\n\n", FontBold));
        return Task.FromResult(titleAndSubtitle);
    }

    private static Task<Paragraph> CreateContent(List<PdfContent> contents, int alignment)
    {
        Paragraph contentsPdf = new Paragraph { Alignment = alignment };

        foreach (var content in contents)
        {
            contentsPdf.Add(new Paragraph(content.Title, FontBoldPlus));
            contentsPdf.Add(new Paragraph(content.Content));

            if (!string.IsNullOrEmpty(content.LinkUrl))
            {
                Anchor link = new Anchor($"\n{content.LinkMessage}") { Reference = content.LinkUrl };
                contentsPdf.Add(link);
            }

            contentsPdf.Add(new Paragraph("\n\n"));
        }

        return Task.FromResult(contentsPdf);
    }

    private static async Task<Paragraph> CreateSignature(PdfParameters paramsPdf)
    {
        Paragraph signature = new Paragraph("\n") { Alignment = Element.ALIGN_CENTER };

        Image imageSignature = Image.GetInstance(paramsPdf.SignatureImage);
        imageSignature.Alignment = Element.ALIGN_CENTER;
        imageSignature.ScaleToFit(200, 100);

        Paragraph signatureLine = await CreateDivider(35);
        Paragraph signatureWord = new Paragraph($"{paramsPdf.SignatureWord}");

        signature.Add(imageSignature);
        signature.Add(signatureLine);
        signature.Add(signatureWord);

        return signature;
    }

    private static Task<Paragraph> CreateDivider(int tam)
    {
        string line = new string('_', tam);
        Paragraph paragraph = new Paragraph(line) { Alignment = Element.ALIGN_CENTER };
        return Task.FromResult(paragraph);
    }
}