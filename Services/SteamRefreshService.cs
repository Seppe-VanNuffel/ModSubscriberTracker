namespace Services;

public class SteamRefreshService
{
    private readonly SteamManager _steamManager;
    private readonly TimeSpan _refreshInterval;

    public event EventHandler? DataUpdated;

    public SteamRefreshService(SteamManager steamManager, TimeSpan refreshInterval)
    {
        _steamManager = steamManager;
        _refreshInterval = refreshInterval;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        // Do an immediate refresh when the application starts.
        await RefreshAsync(cancellationToken);

        using PeriodicTimer timer = new(_refreshInterval);

        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                await RefreshAsync(cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Normal when the application shuts down.
        }
    }

    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _steamManager.UpdateWorkshopItemData();

            DataUpdated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception exception)
        {
            // Don't let a failed Steam request kill the refresh loop.
            Console.WriteLine($"Steam refresh failed: {exception.Message}");
        }
    }
}