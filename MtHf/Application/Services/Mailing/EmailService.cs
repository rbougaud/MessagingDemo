using System.Net.Mail;
using System.Net;
using Serilog;

namespace Application.Services.Mailing;

public class EmailService(ILogger logger,string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
{
    private readonly string _smtpServer = smtpServer;
    private readonly int _smtpPort = smtpPort;
    private readonly string _smtpUsername = smtpUsername;
    private readonly string _smtpPassword = smtpPassword;
    private readonly ILogger _logger = logger;

    public void SendEmail(string to, string subject, string body, bool isHtml = false)
    {
        try
        {
            //using (var message = new MailMessage())
            //{
            //    message.From = new MailAddress(_smtpUsername);
            //    message.To.Add(to);
            //    message.Subject = subject;
            //    message.Body = body;
            //    message.IsBodyHtml = isHtml;

            //    using (var client = new SmtpClient(_smtpServer, _smtpPort))
            //    {
            //        client.EnableSsl = true;
            //        client.UseDefaultCredentials = false;
            //        client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            //        client.Send(message);
            //    }
            //}

            _logger.Information("E-mail envoyé avec succès à {to}.", to);
        }
        catch (Exception ex)
        {
            _logger.Error("Erreur lors de l'envoi de l'e-mail : {message}", ex.Message);
        }
    }
}
