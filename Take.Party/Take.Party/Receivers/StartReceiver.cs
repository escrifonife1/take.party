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
using System.Runtime.Caching;
using System.Collections.Generic;

namespace Take.Party
{
    public class StartReceiver : BaseReceiver, IMessageReceiver
    {
        private readonly IMessagingHubSender _sender;

        public StartReceiver(IMessagingHubSender sender, Settings settings)
            : base(settings)
        {
            _sender = sender;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            if (message.Content.ToString() == "/start")
            {
                await _sender.SendMessageAsync(new PlainText
                {
                    Text = "Bem vindo, digite o nome da música, artista ou album."
                }, message.From, cancellationToken);
                return;
            }

            try
            {
                var lockCheck = Cache.Get($"{message.From}-lock");
                if (lockCheck != null)
                {
                    await _sender.SendMessageAsync(new PlainText
                    {
                        Text = "Vc pediu uma música recentemente, de uma chance para os outros e tente novamente em alguns minutos."
                    }, message.From, cancellationToken);
                    return;
                }

                StateManager.Instance.SetState(message.From.ToNode(), "select-type");
                var item = _spotify.SearchItems(message.Content.ToString(), SearchType.Track | SearchType.Artist | SearchType.Album);

                if (item != null)
                {
                    var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddMinutes(5) };
                    Cache.Add(message.From.ToString(), item, policy);

                    var selectOptions = new List<SelectOption>();

                    if (item.Tracks?.Total > 0)
                    {
                        selectOptions.Add(new SelectOption
                        {
                            Text = $"Músicas",
                            Order = 0,
                            Value = new PlainText { Text = "0" }
                        });
                    }

                    if (item.Albums?.Total > 0)
                    {
                        selectOptions.Add(new SelectOption
                        {
                            Text = $"Albums",
                            Order = 1,
                            Value = new PlainText { Text = "1" }
                        });
                    }

                    if (item.Artists?.Total > 0)
                    {
                        selectOptions.Add(new SelectOption
                        {
                            Text = $"Artistas",
                            Order = 2,
                            Value = new PlainText { Text = "2" }
                        });
                    }

                    var select = new Select
                    {
                        Text = "Escolha o tipo que deseja procurar:",
                        Options = selectOptions.ToArray()
                    };

                    await _sender.SendMessageAsync(select, message.From, cancellationToken);
                }

                Console.WriteLine($"From: {message.From} \tContent: {message.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await _sender.SendMessageAsync("Deu erro... tente de novo", message.From, cancellationToken);
            }
        }
    }
}
