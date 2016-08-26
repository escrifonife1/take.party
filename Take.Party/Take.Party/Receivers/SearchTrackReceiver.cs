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
using Take.Party.Receivers;

namespace Take.Party
{
    public class SearchTrackReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;
        private SpotifyWebAPI _spotify;

        public SearchTrackReceiver(IMessagingHubSender sender)
        {
            _sender = sender;
            _spotify = GetSpotify();
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                //EXEMPLO ESTA AQUI http://johnnycrazy.github.io/SpotifyAPI-NET/SpotifyWebAPI/auth/
                //var _spotify = await GetToken(); //ESSA LINHA É SO PARA PEGAR O TOKEN. DEPOIS DISSO PODE SER COMENTADA
             
                var item = _spotify.SearchItems(message.Content.ToString(), SearchType.Track);
                var tracks = "Não achou";
                if (item.Tracks != null)
                {
                    //tracks = string.Join("\n", item.Tracks?.Items?.Select(i => $"{i?.Name} {i.Artists.First()?.Name}"));
                    //await ShowMediaLinkOptionsAsync(message.From, cancellationToken, item?.Tracks?.Items?.Select(i => new string[][] { $"{i?.Name} {i.Artists?.First()?.Name}", i.Uri }).ToArray());
                    StateManager.Instance.SetState(message.From, nameof(AddTrackToListReceiver));
                    await ShowMediaLinkOptionsAsync(message.From, cancellationToken, item?.Tracks);
                    
                }
                Console.WriteLine($"From: {message.From} \tContent: {message.Content}");
                //await _sender.SendMessageAsync(tracks, message.From, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _sender.SendMessageAsync("Deu erro... tente de novo", message.From, cancellationToken);
            }
        }

        private async Task ShowMediaLinkOptionsAsync(Node from, CancellationToken cancellationToken, Paging<FullTrack> songs)
        {
            var select = new Select
            {
                Text = "Escolha o tipo de MediaLink que deseja receber:"
            };

            var selectOptions = new SelectOption[songs.Items.Count()];
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
            var i = 0;
            foreach (var song in songs.Items)
            {
                selectOptions[i] = new SelectOption
                {
                    Text = $"{song.Name} {song.Artists.First().Name}",
                    Order = i + 1,
                    Value = new PlainText { Text = song.Uri }
                };
                i++;
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
