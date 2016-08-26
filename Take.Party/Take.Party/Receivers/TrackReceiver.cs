using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using SpotifyAPI.Web.Models;
using Lime.Messaging.Contents;
using Takenet.MessagingHub.Client;

namespace Take.Party.Receivers
{
    public class TrackReceiver : BaseReceiver, IMessageReceiver
    {
        private IMessagingHubSender _sender;

        public TrackReceiver(IMessagingHubSender sender)
        {
            _sender = sender;
            

        }
        public async Task ReceiveAsync(Message envelope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var item = Cache.Get(envelope.From.ToString()) as SearchItem;

            var select = new Select
            {
                Text = "Escolha a música que deseja adicionar à playlist:"
            };

            var selectOptions = new List<SelectOption>();
            
            for (int i = 0; i < 5; i++)
            {
                selectOptions.Add( new SelectOption
                {
                    Text = $"{item.Tracks.Items[i].Name} {item.Tracks.Items[i].Artists.First().Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = item.Tracks.Items[i].Uri }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
        
    }
}
