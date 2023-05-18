using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace Client.Core.Abstract;

public interface IHubFactory
{
    Task<HubConnection> CreateHub ();
}