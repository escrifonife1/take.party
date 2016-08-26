using Lime.Protocol;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Party.Receivers;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;

namespace Take.Party
{
    public class ArtistReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender sender;

        public ArtistReceiver(IMessagingHubSender sender)
        {
            this.sender = sender;
        }

        public Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = Cache.Get(envelope.From.ToString()) as SearchItem;

            throw new NotImplementedException();
        }
    }
}
