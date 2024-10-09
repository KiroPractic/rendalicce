using Hangfire.Common;
using Hangfire.Server;
using Rendalicce.Infrastructure.Emails;

namespace Rendalicce.Infrastructure.BackgroundJobs;

public class HangfireJobFailureNotificationFilter : JobFilterAttribute, IServerFilter
{
    private readonly EmailSendingService _emailSendingService;

    public HangfireJobFailureNotificationFilter(EmailSendingService emailSendingService) => _emailSendingService = emailSendingService;

    public void OnPerforming(PerformingContext filterContext)
    {
        // No action before the job execution
    }

    public async void OnPerformed(PerformedContext filterContext)
    {
        if (filterContext.Exception is not null)
        {
            var job = filterContext.BackgroundJob.Job;
            var jobName = $"{job.Method.DeclaringType?.Name}.{job.Method.Name}";

            try
            {
                await _emailSendingService.SendHangfireFailureNotification(filterContext.BackgroundJob.Id, jobName, filterContext.BackgroundJob.ParametersSnapshot, filterContext.Exception);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}