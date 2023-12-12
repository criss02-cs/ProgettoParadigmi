using ProgettoParadigmi.Api.Business;
using ProgettoParadigmi.EmailSender;
using ProgettoParadigmi.Models.Context;

namespace ProgettoParadigmi.Api.BackgroundService
{
    public class ReminderService
        : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromHours(24);
        private readonly AppuntamentiManager _appuntamentiManager;

        public ReminderService(IServiceScopeFactory serviceScopeFactory, EmailService emailService)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppuntamentiDbContext>();
            if (context != null) _appuntamentiManager = new AppuntamentiManager(context, emailService);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);
            try
            {
                while (
                    !stoppingToken.IsCancellationRequested &&
                    await timer.WaitForNextTickAsync(stoppingToken)
                )
                {
                    await _appuntamentiManager.InviaReminderEmail();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
