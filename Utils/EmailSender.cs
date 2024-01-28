using VirtualLibrary.Interfaces;

namespace VirtualLibrary.Utils;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(
        string fromAdrress,
        string destinationAddress,
        string subject,
        string textMessage
    ) {
        await Task.CompletedTask;
    }
}