using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Interfaces;
using Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Models;

namespace Same_Photo_Malcolm_Turnbull_Everyday_Discord_Bot.Helpers;

public class TaskSchedulerService : BackgroundService 
{
    private readonly ILogger<TaskSchedulerService> _logger;
    private readonly AppConfiguration _appConfiguration;
    private readonly IBackendTurnbullImage _backendTurnbullImage;
    
    public TaskSchedulerService(ILogger<TaskSchedulerService> logger, AppConfiguration appConfiguration, IBackendTurnbullImage backendTurnbullImage)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
        _backendTurnbullImage = backendTurnbullImage;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Daily Task Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now;
                // Currently set to run at 8:10PM
                var nextRunTime = now.Date.AddHours(20).AddMinutes(10);

                if (now > nextRunTime)
                {
                    nextRunTime = nextRunTime.AddDays(1);
                }

                var delay = nextRunTime - now;
                _logger.LogInformation($"Next daily task scheduled at: {nextRunTime} (Delay: {delay.TotalMinutes} minutes)");


                await Task.Delay(delay, stoppingToken);
                
                await SendTurnbullImageTask();

            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Daily Task Service is stopping gracefully.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the daily task.");
            }
        }
    }

    private async Task SendTurnbullImageTask()
    {
        var taskId = Guid.NewGuid();
        _logger.LogInformation("Executing RunSendTurnbullImageTask for channel : " + _appConfiguration.ChannelId + " TaskId: " + taskId + "at " + DateTime.Now);
        
        var channelId = _appConfiguration.ChannelId;
        await Task.Run(() =>
        {
            try
            {
                _backendTurnbullImage.SendMalcolmTurnbullPhoto(channelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing RunSendTurnbullImageTask.");
            }
            
        });

        _logger.LogInformation("RunSendTurnbullImageTask execution completed for TaskId: " + taskId + " at " + DateTime.Now);
    }
}