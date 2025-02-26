namespace BookItApi.Services;

/// <summary>
/// Interface para enviar Email
/// </summary>
public interface IEmailSender {

    /// <summary>
    /// método da interface
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="htmlMessage"></param>
    /// <returns></returns>
    Task SendEmailAsync(string to, string subject, string htmlMessage);
}