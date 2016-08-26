using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using System.Linq;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web;
using Lime.Messaging.Contents;
using Take.Party.Receivers;

namespace Take.Party
{
    public class AudioSearchTrackReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;
        private SpotifyWebAPI _spotify;

        public AudioSearchTrackReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            _sender = sender;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("");
        }
    }
}
