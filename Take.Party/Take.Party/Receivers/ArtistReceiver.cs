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
    public class ArtistReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;

        public ArtistReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            this._sender = sender;
        }

        public async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = Cache.Get(envelope.From.ToString()) as SearchItem;
            var select = new Select
            {
                Text = "Escolha o artista:"
            };

            var selectOptions = new List<SelectOption>();

            var count = item.Artists.Items.Count > 5 ? 5 : item.Artists.Items.Count;
            for (int i = 0; i < count; i++)
            {
                selectOptions.Add(new SelectOption
                {
                    Text = $"{item.Artists.Items[i].Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = $"artista {item.Artists.Items[i].Id}" }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
    }
}
