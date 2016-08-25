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
using SpotifyAPI.Web;

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
            //var _spotify = await GetToken(); //ESSA LINHA É SO PARA PEGAR O TOKEN. DEPOIS DISSO PODE SER COMENTADA

            using (var _spotify = new SpotifyWebAPI
            {
                AccessToken = "BQCpkZHiFLRt7C2pkNjpTx3fGLwKvh2-UU-ynU09AP-qKpNipq3j0P6ln1_UvzJPRWoe2VVAe6ipayBUhONj4-BaHxtewb7qSuOf8l1I8CrZkBr5YOWBMAAq0mUdFHTDBZoGlc9vsaJzcPwkDzulVGDH8-BPlKEEejGemYUkcrniOwlV6F33nS5C_1ugHk_pF5I6NHL_XPhIdg_tGdk_e4TogExzocKNs_i-6HlMXrZNGh88yRsPC6hIQq0sT2UKOkoBBwBB9FW_fHU4OEQw_akCABv0Dn18L_w",
                TokenType = "Bearer"
            })
            {
                var item = _spotify.SearchItems(message.Content.ToString(), SearchType.Track);
                var tracks = "Não achou";
                if (item.Tracks != null)
                {
                    tracks = string.Join("\n", item.Tracks?.Items.Select(i => i.Name));
                }
                Console.WriteLine($"From: {message.From} \tContent: {message.Content}");
                await _sender.SendMessageAsync(tracks, message.From, cancellationToken);
            }
        }

        private async Task<SpotifyWebAPI> GetToken()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                   "http://localhost",
                   8000,
                   "4ba8934628a54571b57ed84de51d1825",
                   Scope.UserReadPrivate,
                   TimeSpan.FromSeconds(20)
              );
            SpotifyWebAPI _spotify = null;
            try
            {                
                _spotify = await webApiFactory.GetWebApi();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return _spotify;
        }
    }
}
