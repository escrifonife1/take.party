using SpotifyAPI.Web;
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

        protected BaseReceiver()
        {
            _spotify = new SpotifyWebAPI()
            {
                AccessToken = "BQDG1eUfCR1-jzy7UZHxo11jyr4oIQODS_I4w6FOmFPwoOoQactxQnzFn56DB6w-ItBMcDASZ3liZobgMXwCl_XKMZ1XsaIb26Yb0-Ll6HHbfgKCwnd9F8q9IHPcm8jyBw8D7J_D9EovCMDHPT9nmW8A6UGjhxl0HLaQvj_IBVUy7VRPu04x2gMxXZu7AHL79MAmR8eGGqFmdvcUyOSAG0b_FdXmGrSmKosVuEbiTAnSp5CzPQoeZeRd-oBiUUNTtTHiO_eAhMJdC4KbcveYfuKo4_qUi1zGrfk",
                TokenType = "Bearer"
            };
        }
    }
}
