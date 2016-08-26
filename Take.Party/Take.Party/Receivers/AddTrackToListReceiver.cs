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
        private SpotifyWebAPI _spotify;

        public AddTrackToListReceiver(IMessagingHubSender sender)
        {
            _sender = sender;
            _spotify = GetSpotify();
            _profile = _spotify.GetPrivateProfile();
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            //var playlist =  _spotify.GetUserPlaylists(_profile.Id);
            await _sender.SendMessageAsync("Sua musica foi adicionada", message.From, cancellationToken);
            //StateManager.Instance.SetState(message.From, nameof(SearchTrackReceiver));
            //await _spotify.AddPlaylistTrackAsync(_profile.Id, "5bQ5OtlOIx9ADm4BtWJ6yx", "spotify:user:take.party");
        }
    }
}
