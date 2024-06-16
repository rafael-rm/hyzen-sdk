using Hyzen.SDK.Authentication;
using Hyzen.SDK.Cloudflare;
using Hyzen.SDK.PdfGenerator;
using Hyzen.SDK.PdfGenerator.DTO.Pdf;
using iTextSharp.text;

namespace Hyzen.Console;

internal abstract class Program
{
    static async Task Main()
    {
        var r2 = R2.Get("hyzen-temporary");
        
        var result1 = await r2.DeleteObject("cf0e305e-fcd6-43dd-9369-814011f39a3b.pdf");
        var result2 = await r2.GetObject("f7121d9c-c4d2-4750-a974-bf1527294115.pdf");
        var result3 = await r2.ListBuckets();
        var result4 = await r2.ListObjects();
        
        PdfParameters parameters = new PdfParameters();
        parameters.HeaderList?.Add("Responsável: Rafael Rodrigues");
        parameters.HeaderList?.Add("Empresa: XPTO");
        parameters.HeaderList?.Add($"Atendimento: {Guid.NewGuid()}");
        parameters.HeaderList?.Add($"Protocolo: {Guid.NewGuid()}");
        parameters.HeaderList?.Add($"Data: {DateTime.Now.AddHours(-6)}");

        parameters.Logo = await File.ReadAllBytesAsync("logo.png");

        parameters.TitleDocument = "Titulo do Arquivo";
        parameters.SubTitleDocument = $"{DateTime.Now}";

        parameters.SignatureImage = await File.ReadAllBytesAsync("assinatura.png");
        parameters.SignatureWord = "Assinatura do Responsável";

        List<PdfContent> contents = new List<PdfContent>()
        {
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 1",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla."
            },
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 2",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed auctor auctor nisi eu pharetra. Sed ac nunc vel purus congue tincidunt at eu ipsum. Ut eleifend consectetur magna, eu interdum libero ultrices quis. Aenean eu dapibus neque. Nulla facilisi. Suspendisse vel volutpat felis, a tincidunt justo. Vivamus a turpis lacinia, finibus tortor sit amet, dignissim dui."
            },
            new()
            {
                Title = "Titulo 3",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque sit amet interdum elit. Maecenas quis accumsan nulla, eu convallis justo. Proin a nunc ut elit posuere vestibulum a nec dui. Suspendisse tempor non purus a faucibus. Sed finibus sagittis ligula, a auctor nulla interdum eu. Nulla facilisi. Ut quis aliquet justo, nec finibus massa. Sed hendrerit enim quis rhoncus imperdiet."
            },
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 4",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce eget leo eu purus consectetur finibus eu sit amet justo. Nam interdum, justo eu vestibulum posuere, metus felis volutpat velit, vitae dictum nisl elit ut lorem. Nulla facilisi. In pharetra pellentesque neque, ac varius ipsum vulputate eu. Praesent interdum elit eu mi feugiat, vel dictum purus tincidunt."
            },
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 5",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam non mauris eu neque aliquam dictum vel vel tellus. Suspendisse potenti. Vestibulum in velit condimentum, dignissim mauris eget, dignissim ligula. Vivamus pharetra, arcu eget maximus blandit, dolor erat condimentum orci, quis aliquet risus turpis in nunc. Sed lacinia felis eu justo rhoncus elementum."
            },
            new()
            {
                Title = "Titulo 6",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur egestas, risus eget egestas laoreet, ex felis congue mauris, quis cursus erat elit sit amet lectus. Sed quis tempus dui, nec ultrices metus. Maecenas iaculis diam in neque elementum, nec scelerisque tellus consequat. Pellentesque sed tristique velit. Sed eu nibh sem."
            },
            new()
            {
                Title = "Titulo 7",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris convallis, elit sit amet volutpat malesuada, nisi mi pellentesque felis, nec aliquet sapien turpis nec mauris. Aliquam sit amet metus eu nunc facilisis ultrices. Etiam ac ligula ex. Integer suscipit quam ac ligula venenatis, eget tincidunt elit finibus."
            },
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 8",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed ultrices diam et interdum lobortis. Nullam egestas luctus sagittis. Suspendisse potenti. Curabitur non ligula nec purus vestibulum aliquam. Pellentesque efficitur elementum felis at fringilla. In euismod, mi a lacinia faucibus, turpis neque fringilla odio, nec facilisis nulla ligula nec odio."
            },
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 9",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque scelerisque metus ut nulla cursus, a sodales augue vulputate. Vivamus varius tortor eu quam hendrerit, sit amet auctor mi hendrerit. Fusce vitae semper augue. Integer scelerisque dolor eu velit blandit cursus. Suspendisse eu lobortis elit, in faucibus justo. Etiam sit amet pellentesque felis."
            },
            new()
            {
                LinkMessage = "Clique aqui para acessar a imagem", LinkUrl = "https://google.com/", Title = "Titulo 10",
                Content =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam consectetur est a magna luctus, ut varius metus viverra. Suspendisse a felis id purus scelerisque eleifend. Sed ultrices metus a mauris finibus tincidunt. Pellentesque id tristique libero. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Nam sit amet sem vel ex laoreet viverra."
            }
        };

        parameters.ContentList = contents;
        
        parameters.ContentsAlignment = Element.ALIGN_LEFT;

        byte[] pdfBytes = await PdfCreator.Create(parameters);
        
        Guid guid = Guid.NewGuid();
        string namePdf = String.Concat(guid, ".pdf");
        
        var upload = await r2.PutObject(namePdf, pdfBytes);
        var link = await r2.GeneratePresignedUrl(namePdf, DateTime.Now.AddHours(1));
    }
}