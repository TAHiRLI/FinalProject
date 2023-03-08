using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using MedlabApi.Services.Implementations;
using Quartz;

namespace MedlabApi.Jobs
{
    public class EmailSendingJob : IJob
    {
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionRepostiory _subscriptionRepostiory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EmailSendingJob(IConfiguration configuration, ISubscriptionRepostiory subscriptionRepostiory, IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _subscriptionRepostiory = subscriptionRepostiory;
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var subscriptionRepostiory = scope.ServiceProvider.GetRequiredService<ISubscriptionRepostiory>();
                var subscriptionService = new SubscriptionService(subscriptionRepostiory, _configuration);
                subscriptionService.SendEmailsToSubscribers();
            }
            return Task.CompletedTask;
        }
    }
}
