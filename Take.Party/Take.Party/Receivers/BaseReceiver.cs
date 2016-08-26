using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Take.Party.Receivers
{
    public abstract class BaseReceiver
    {
        public static MemoryCache Cache = new MemoryCache("bot");
        protected SpotifyWebAPI _spotify;
        protected Settings _settings;
        protected BaseReceiver(Settings settings)
        {
            _settings = settings;
            _spotify = new SpotifyWebAPI()
            {
                AccessToken = settings.AccessToken,
                TokenType = "Bearer"
            };

            var profile = _spotify.GetPrivateProfile();

            if (profile.Error != null)
            {
                var auth = new SpotifyAPI.Web.Auth.AutorizationCodeAuth()
                {
                    ClientId = settings.ClientId,
                    Scope = Scope.UserReadPrivate | Scope.PlaylistModifyPublic | Scope.PlaylistModifyPrivate,
                    RedirectUri = "http://localhost:8000"
                };

                var token = auth.RefreshToken(settings.RefreshToken, settings.ClientSecret);

                settings.AccessToken = token.AccessToken;
                _spotify.AccessToken = token.AccessToken;
            }
        }
    }
}
