using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Take.Party.Receivers;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using SpotifyAPI.Web.Models;
using Lime.Messaging.Contents;
using Takenet.MessagingHub.Client;

namespace Take.Party
{
    public class SelectAlbumReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;

        public SelectAlbumReceiver(IMessagingHubSender sender)
        {
            this._sender = sender;
        }

        public async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = Cache.Get(envelope.From.ToString()) as SearchItem;

            var id = envelope.Content.ToString().Split(' ')[1];

            var tracks = await _spotify.GetAlbumTracksAsync(id);

            var select = new Select
            {
                Text = "Escolha o album:"
            };

            var selectOptions = new List<SelectOption>();

            var count = tracks.Items.Count > 5 ? 5 : tracks.Items.Count;

            for (int i = 0; i < count; i++)
            {
                selectOptions.Add(new SelectOption
                {
                    Text = $"{tracks.Items[i].Name} {tracks.Items[i].Artists.First().Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = $"track {tracks.Items[i].Id}" }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
    }
}
