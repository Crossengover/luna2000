using Job_server.Options;
using Microsoft.Extensions.Options;

namespace Job_server.Jobs;

public class DeductRentJob
{
    private readonly ConnectionConfiguration _connectionOptions;

    public DeductRentJob(IOptions<ConnectionConfiguration> connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    public async Task DeductRent()
    {
        using var client = new HttpClient();
        await client
            .PostAsync($"http://{_connectionOptions.BaseUrl}:{_connectionOptions.Port}/api/deduct-rent", null);
    }
}