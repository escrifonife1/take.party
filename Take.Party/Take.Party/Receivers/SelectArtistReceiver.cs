using Lime.Messaging.Contents;
using Lime.Protocol;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Party.Receivers;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;

namespace Take.Party
{
    public class SelectArtistReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;

        public SelectArtistReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            this._sender = sender;
        }

        public async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var id = envelope.Content.ToString().Split(' ')[1];

            var tracks = await _spotify.GetArtistsTopTracksAsync(id, "BR");

            var select = new Select
            {
                Text = "Escolha a música que deseja adicionar à playlist:"
            };

            var selectOptions = new List<SelectOption>();

            var count = tracks.Tracks.Count > 5 ? 5 : tracks.Tracks.Count;
            for (int i = 0; i < count; i++)
            {
                selectOptions.Add(new SelectOption
                {
                    Text = $"{tracks.Tracks[i].Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = $"track {tracks.Tracks[i].Uri}" }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
    }
}
