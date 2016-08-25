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
using SpotifyAPI.Web.Auth;

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
            //EXEMPLO ESTA AQUI http://johnnycrazy.github.io/SpotifyAPI-NET/SpotifyWebAPI/auth/
            await GetToken(); //ESSA LINHA É SO PARA PEGAR O TOKEN. DEPOIS DISSO PODE SER COMENTADA
                        
            var _spotify = new SpotifyAPI.Web.SpotifyWebAPI();
            _spotify.AccessToken = "COLOCAR O TOKEN GERADO AQUI";
                var item = _spotify.SearchItems(message.Content.ToString(), SearchType.Track);
            var tracks = "Não achou";
            if (item.Tracks != null)
            {
                tracks = string.Join("\n", item.Tracks?.Items.Select(i => i.Name));
            }
            Console.WriteLine($"From: {message.From} \tContent: {message.Content}");
            await _sender.SendMessageAsync(tracks, message.From, cancellationToken);
        }

        private async Task GetToken()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                   "http://requestb.in/176gbee1",
                   8000,
                   "6371ccb6dc9e44069bca6763bcb539b6",
                   Scope.UserReadPrivate,
                   TimeSpan.FromSeconds(20)
              );
            var _spotify = await webApiFactory.GetWebApi();
        }
    }
}
