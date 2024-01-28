namespace VirtualLibrary.Interfaces;

public interface IEmailSender {
    Task SendEmailAsync(
        string fromAdrress,
        string destinationAddress,
        string subject,
        string textMessage
    );
}