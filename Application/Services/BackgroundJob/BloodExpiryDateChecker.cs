
using BloodHeroA.Application.Services.Interfaces;

namespace BloodHeroA.Application.Services.BackgroundJob
{
    public class BloodExpiryDateChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeService;

        public BloodExpiryDateChecker(IServiceScopeFactory scopeService)
        {
            _scopeService = scopeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeService.CreateScope();
                var bloodService = scope.ServiceProvider.GetRequiredService<IBloodStorageService>();
                await bloodService.UpdateExpiredBloodCountAsync();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
