using Microsoft.Extensions.Options;
using NetForemost.Core.Entities.Emails;
using System.Net;
using System.Net.Mail;

namespace NetForemost.Core.Services.Email;

public interface ISmtpClient
{
    public Task SendMailAsync(MailMessage message);
}


public class SmtpClientWrapper : ISmtpClient
{
    private SmtpClient _smtpClient = new();
    private readonly SmtpClientConfig _smtpClientConfig;

    public SmtpClientWrapper(IOptions<SmtpClientConfig> options)
    {
        _smtpClientConfig = options.Value;

        _smtpClient.Host = _smtpClientConfig.Host;
        _smtpClient.Port = _smtpClientConfig.Port;
        _smtpClient.EnableSsl = _smtpClientConfig.EnableSsl;
        _smtpClientConfig.UseDefaultCredentials = _smtpClientConfig.UseDefaultCredentials;
        _smtpClient.Credentials = new NetworkCredential(_smtpClientConfig.UserName, _smtpClientConfig.Password);
    }

    public Task SendMailAsync(MailMessage message)
    {
        return _smtpClient.SendMailAsync(message);
    }
}
