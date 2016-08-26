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

        public SelectAlbumReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            this._sender = sender;
        }

        public async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var id = envelope.Content.ToString();

            var tracks = await _spotify.GetAlbumTracksAsync(id);

            var select = new Select
            {
                Text = "Escolha a música que deseja adicionar à playlist:"
            };

            var selectOptions = new List<SelectOption>();

            var count = tracks.Items.Count > 5 ? 5 : tracks.Items.Count;

            for (int i = 0; i < count; i++)
            {
                selectOptions.Add(new SelectOption
                {
                    Text = $"{tracks.Items[i].Name} {tracks.Items[i].Artists.First().Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = $"{tracks.Items[i].Uri}" }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
    }
}
