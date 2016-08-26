using Lime.Protocol;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Party.Receivers;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using SpotifyAPI.Web.Models;
using Takenet.MessagingHub.Client;

namespace Take.Party
{
    public class AddTrackToListReceiver : BaseReceiver, IMessageReceiver
    {
        private PrivateProfile _profile;
        private readonly IMessagingHubSender _sender;

        public AddTrackToListReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            _sender = sender;
            _profile = _spotify.GetPrivateProfile();
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            Cache.Remove(message.From.ToString());

            await _spotify.AddPlaylistTrackAsync(_profile.Id, _settings.PlayListId, message.Content.ToString());
            Cache.Add($"{message.From}-lock", true, DateTime.Now.AddMinutes(_settings.Timeout));
            await _sender.SendMessageAsync("Sua musica foi adicionada a playlist.", message.From, cancellationToken);
        }
    }
}
