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

namespace Take.Party
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;

        public PlainTextMessageReceiver(IMessagingHubSender sender)
        {
            _sender = sender;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            var _spotify = new SpotifyAPI.Web.SpotifyWebAPI();
            _spotify.AccessToken = "";
            var item = _spotify.SearchItems(message.Content.ToString(), SearchType.Track);
            var tracks = "Não achou";
            if (item.Tracks != null)
            {
                tracks = string.Join("\n", item.Tracks?.Items.Select(i => i.Name));
            }
            Console.WriteLine($"From: {message.From} \tContent: {message.Content}");
            await _sender.SendMessageAsync(tracks, message.From, cancellationToken);
        }
    }
}
