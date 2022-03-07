using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace YoutubeDownloader.Infrastructure.Hubs
{
    public class MediaSendingHub : Hub<IMediaSendingsHub>
    {

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task ReportProgress(double progress)
        {
                await Clients.Client(Context.ConnectionId).ReportProcessingProgress(progress);
            
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }

    public interface IMediaSendingsHub
    {
        Task ReportProcessingProgress(double progress);
    }
}
