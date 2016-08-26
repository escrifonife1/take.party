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
using Lime.Messaging.Contents;

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
            try
            {
                //EXEMPLO ESTA AQUI http://johnnycrazy.github.io/SpotifyAPI-NET/SpotifyWebAPI/auth/
                //var _spotify = await GetToken(); //ESSA LINHA É SO PARA PEGAR O TOKEN. DEPOIS DISSO PODE SER COMENTADA
                //StateManager.Instance.SetState(message.From, Bot.Settings.States.WaitingTaskTime);
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
                        //tracks = string.Join("\n", item.Tracks?.Items?.Select(i => $"{i?.Name} {i.Artists.First()?.Name}"));
                        await ShowMediaLinkOptionsAsync(message.From, cancellationToken, item?.Tracks?.Items?.Select(i => $"{i?.Name} {i.Artists?.First()?.Name}").ToArray());
                    }
                    Console.WriteLine($"From: {message.From} \tContent: {message.Content}");
                    //await _sender.SendMessageAsync(tracks, message.From, cancellationToken);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _sender.SendMessageAsync("Deu erro... tente de novo", message.From, cancellationToken);
            }
        }

        private async Task ShowMediaLinkOptionsAsync(Node from, CancellationToken cancellationToken, string[] songs)
        {
            var select = new Select
            {
                Text = "Escolha o tipo de MediaLink que deseja receber:"
            };

            var selectOptions = new SelectOption[10];
            //var optionsLabel = new string[] {
            //    "Imagem",
            //    "Imagem - Media Bucket",
            //    "Imagem - Sem preview",
            //    "Imagem - Tipo preview inválido",
            //    "Imagem - Url inválida",
            //    "Imagem - Tamanho e Tipo inválido",
            //    "Audio",
            //    "Audio - Media Bucket",
            //    "Video",
            //    "Video - Media Bucket"
            //};
                       

            for (int i = 0; i < selectOptions.Length; i++)
            {

                selectOptions[i] = new SelectOption
                {
                    Text = songs[i],
                    Order = i + 1,
                    Value = new PlainText { Text = songs[i] }
                };

            }

            select.Options = selectOptions;

            await _sender.SendMessageAsync(select, from, cancellationToken);
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
