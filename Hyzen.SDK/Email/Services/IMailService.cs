namespace Hyzen.SDK.Email.Services;

public interface IMailService
{
    public Task<bool> SendTemplateEmail(string from, string to, string templateId, object data);
}