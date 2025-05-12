using clippr.Core.Clip;
using Microsoft.Extensions.Options;
using NCrontab;

namespace clippr.API.Background.CleanUp;

public class CleanupService : BackgroundService
{
    private readonly CleanupOptions _cleanUpConfiguration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CleanupService> _logger;

    public CleanupService(IOptions<CleanupOptions> cleanUpConfigurationOptions, IServiceProvider serviceProvider, ILogger<CleanupService> logger)
    {
        _cleanUpConfiguration = cleanUpConfigurationOptions.Value;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && _cleanUpConfiguration.Enabled)
        {
            var timeUntilNextExecution = CrontabSchedule.Parse(_cleanUpConfiguration.CronExpression).GetNextOccurrence(DateTime.Now) - DateTime.Now;
            await Task.Delay(timeUntilNextExecution, stoppingToken);
            try
            {
                await Task.Run(RunCleanup, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured during the cleanup: {message}", e.Message);
            }
        }
        throw new NotImplementedException();
    }

    private void RunCleanup()
    {
        using var scope = _serviceProvider.CreateScope();
        var clipService = scope.ServiceProvider.GetRequiredService<IClipService>();
        var deletedClips = clipService.CleanUp(TimeSpan.FromHours(_cleanUpConfiguration.MaxClipAgeHours));
        _logger.LogInformation("Cleanup removed {count} clips.", deletedClips);
    }
}