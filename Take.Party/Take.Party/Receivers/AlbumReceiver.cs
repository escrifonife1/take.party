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
    public class AlbumReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;

        public AlbumReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            this._sender = sender;
        }

        public async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = Cache.Get(envelope.From.ToString()) as SearchItem;
            var select = new Select
            {
                Text = "Escolha o album:"
            };

            var selectOptions = new List<SelectOption>();

            var count = item.Albums.Items.Count > 5 ? 5 : item.Albums.Items.Count;
            for (int i = 0; i < count; i++)
            {
                selectOptions.Add(new SelectOption
                {
                    Text = $"{item.Albums.Items[i].Name} {item.Albums.Items[i].Type}",
                    Order = i + 1,
                    Value = new PlainText { Text = $"album {item.Albums.Items[i].Id}" }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
    }
}
