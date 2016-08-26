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
using MoreLinq;

namespace Take.Party.Receivers
{
    public class TrackReceiver : BaseReceiver, IMessageReceiver
    {
        private IMessagingHubSender _sender;

        public TrackReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
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

            var tracks = item.Tracks.Items.DistinctBy(x => $"{x.Name} {x.Artists.First().Name}").ToList();

            var count = tracks.Count > 5 ? 5 : tracks.Count;

            for (int i = 0; i < count; i++)
            {
                selectOptions.Add( new SelectOption
                {
                    Text = $"{tracks[i].Name} {tracks[i].Artists.First().Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = $"track {tracks[i].Uri}" }
                });
            }

            select.Options = selectOptions.ToArray();

            await _sender.SendMessageAsync(select, envelope.From, cancellationToken);
        }
        
    }
}
