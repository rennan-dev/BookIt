
using System.Net;
using System.Net.Mail;

namespace BookItApi.Services;

/// <summary>
/// Classe para o envio de email
/// </summary>
public class EmailSender : IEmailSender {

    private readonly IConfiguration _configuration;

    /// <summary>
    /// contrutor para o envio de email
    /// </summary>
    /// <param name="configuration"></param>
    public EmailSender(IConfiguration configuration) {
        _configuration = configuration;
    }

    /// <summary>
    /// Método para o envio de email
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="htmlMessage"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task SendEmailAsync(string to, string subject, string htmlMessage) {
        var smtpSettings = _configuration.GetSection("Smtp");
        var smtpClient = new SmtpClient(smtpSettings["Host"]) {
            Port = int.TryParse(smtpSettings["Port"], out int port) ? port : 587, //se falhar, 587 é um padrão comum
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = true
        };

        var mailMessage = new MailMessage {
            From = new MailAddress(smtpSettings["From"] ?? throw new InvalidOperationException("Endereço de e-mail do remetente não configurado.")),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}