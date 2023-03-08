using Medlab.Core.Repositories;
using MedlabApi.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;

namespace MedlabApi.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepostiory _subscriptionRepostiory;
        private readonly IConfiguration _configuration;

        public SubscriptionService(ISubscriptionRepostiory subscriptionRepostiory, IConfiguration configuration)
        {
            _subscriptionRepostiory = subscriptionRepostiory;
            _configuration = configuration;
        }
        public void SendEmailsToSubscribers()
        {
            var subscribers = _subscriptionRepostiory.GetAll(x => true);

            EmailService emailService = new EmailService(_configuration);
            foreach (var subscriber in subscribers)
            {
                emailService.SendMail(subscriber.Email, "Email Service Auto Mail", "This is a mail that is sent by online email service");
                subscriber.LastSentAt = DateTime.UtcNow.AddHours(4);
            }

            _subscriptionRepostiory.Commit();
        }
    }
}
