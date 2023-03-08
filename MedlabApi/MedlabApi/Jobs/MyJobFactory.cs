using Quartz;
using Quartz.Spi;

namespace MedlabApi.Jobs
{
    public class MyJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MyJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                return (IJob)scope.ServiceProvider.GetService(bundle.JobDetail.JobType);
            }
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
