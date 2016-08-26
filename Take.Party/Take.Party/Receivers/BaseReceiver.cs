using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Take.Party.Receivers
{
    public abstract class BaseReceiver
    {
        public SpotifyWebAPI GetSpotify()
        {
            var spotify = new SpotifyWebAPI()
            {
                AccessToken = "BQBhsFvZ_T3wXLPH0n0t0FyIXjk87bxB_4z7N7K_rxVE15K340MZzox06NJHCuq8xH-RsmfqVxfJzOcfjFc08v4bPoKSNiRgBtWVAEeISTVk671x8sYzWQGhUQXmrSIaLme4CxxxLzrzLpH-neziEZUQDb2Bfk4T91q0nK_gXPe5ANvHKNXAG_NyAFgIG2QP-8MFCUR3qe4pvX2mavYskvCe54EBvvQu87mm7fehor2IpFRUl18PpeKcyWZWpU8azG647WR5x-EO0f0Skm-n9MVO__r7TI6ucF4",
                TokenType = "Bearer"
            };
            return spotify;
        }
    }
}
