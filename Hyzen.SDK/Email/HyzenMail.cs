using Hyzen.SDK.Email.Services;

namespace Hyzen.SDK.Email;

public static class HyzenMail
{
    private static readonly IMailService Service;

    static HyzenMail()
    {
        Service = new SendGridService();
    }

    public static async Task<bool> SendTemplateMail(string from, string to, string templateId, object data)
    {
        return await Service.SendTemplateEmail(from, to, templateId, data);
    }
}