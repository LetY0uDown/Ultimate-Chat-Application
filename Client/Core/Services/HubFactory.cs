using Client.Core.Abstract;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Client.Core.Services;

internal sealed class HubFactory : IHubFactory
{
    private readonly IConfiguration _config;

    public HubFactory (IConfiguration config)
    {
        _config = config;
    }

    public async Task<HubConnection> CreateHub ()
    {
        var connection = new HubConnectionBuilder()
                             .WithUrl(_config["HostURL"] + "MainHub")
                             .WithAutomaticReconnect()
                             .Build();

        await connection.StartAsync();

        return connection;
    }
}