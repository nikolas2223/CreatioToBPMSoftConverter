using WebWithFileApiExample.Constants;

namespace WebWithFileApiExample.HostedServices
{
    /// <summary>
    /// Сервис для очистки папки с уже отработанными пакетами
    /// </summary>
    public class EraseDirectoryService: IHostedService, IDisposable
    {
        private ILogger<EraseDirectoryService> _logger;
        private int ExecutionCount = 0;
        private Timer? Timer = null;

        public EraseDirectoryService(ILogger<EraseDirectoryService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Timer = new Timer(ErasePackageDirectory, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(10));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }

        private async void ErasePackageDirectory(object? state)
        {
            Interlocked.Increment(ref ExecutionCount);

            try
            {
                var directoryInfo = new DirectoryInfo(PathConstants.TempPackagePath);

                foreach (var file in directoryInfo.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach (var directory in directoryInfo.EnumerateDirectories())
                {
                    directory.Delete(true);
                }
            }
            catch (Exception ioException)
            {
                _logger.LogInformation(ioException.Message);
                _logger.LogError(ioException.ToString());
            }
        }

    }
}
