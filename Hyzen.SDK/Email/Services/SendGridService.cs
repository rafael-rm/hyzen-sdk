﻿using System.Net;
using Hyzen.SDK.Exception;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Hyzen.SDK.Email.Services;

public class SendGridService : IMailService
{
    private static string ApiKey { get; set; }
    private static SendGridClient Client { get; set; }
    
    public SendGridService()
    {
        ApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        
        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new HException("SendGrid API key not found", ExceptionType.InvalidOperation);
        
        Client = new SendGridClient(ApiKey);
    }

    public async Task<bool> SendTemplateEmail(string from, string to, string templateId, object data)
    {
        var gridFrom = new EmailAddress(from, "Hyzen");
        var gridTo = new EmailAddress(to);
        
        var msg = MailHelper.CreateSingleTemplateEmail(gridFrom, gridTo, templateId, data);
        var response = await Client.SendEmailAsync(msg);
        
        if (response.StatusCode != HttpStatusCode.Accepted)
            throw new HException("Failed to send recovery email", ExceptionType.InvalidOperation);
        
        return true;
    }
}