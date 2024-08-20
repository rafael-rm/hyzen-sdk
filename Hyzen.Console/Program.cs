using Hyzen.SDK.Authentication;
using Hyzen.SDK.Authentication.DTO;
using Hyzen.SDK.Cloudflare;
using Hyzen.SDK.Email;
using Hyzen.SDK.SecretManager;

namespace Hyzen.Console;

internal abstract class Program
{
    static async Task Main()
    {
        var key = await HyzenSecret.GetSecretAsync("HYZEN-SDK-SENDGRID-API-KEY");
        
        var dynamicTemplateData = new
        {
            displayName = "Tester",
            recoveryUrl = "https://hyzen.com.br/"
        };
        
        var x = await HyzenMail.SendTemplateMail("noreply@hyzen.com.br", "rafael.rmo@outlook.com", "d-22d1b8b0c8df4ce9ae516cf171a1fa58", dynamicTemplateData);
        
        var subjectTest = new AuthSubject
        {
            Roles = new List<string>
            {
                "hyzen_auth:user:manage_group",
            }
        };

        System.Console.WriteLine(subjectTest.HasRole("hyzen_auth"));
        System.Console.WriteLine(subjectTest.HasRole("hyzen_auth:user"));
        System.Console.WriteLine(subjectTest.HasRole("hyzen_auth:user:manage_group"));
        System.Console.WriteLine(subjectTest.HasRole("hyzen_auth:admin"));
        System.Console.WriteLine(subjectTest.HasRole("hyzen_auth:admin:manage"));
        System.Console.WriteLine(subjectTest.HasRole("hyzen_auth:user:teste"));
        System.Console.WriteLine(subjectTest.HasRole("other_auth:user:view"));
        System.Console.WriteLine(subjectTest.HasRole("other_auth:user:manage"));
        
        
        HyzenAuth.SetToken("test");
        var subject = await HyzenAuth.GetSubject();
        
        var r2 = R2.Get("hyzen-temporary");
        
        var result1 = await r2.DeleteObject("cf0e305e-fcd6-43dd-9369-814011f39a3b.pdf");
        var result2 = await r2.GetObject("f7121d9c-c4d2-4750-a974-bf1527294115.pdf");
        var result3 = await r2.ListBuckets();
        var result4 = await r2.ListObjects();
    }
}