namespace MedlabApi.Services.Interfaces
{
    public interface IEmailService
    {
        void SendMail(string To, string subject, string message);
    }
}
