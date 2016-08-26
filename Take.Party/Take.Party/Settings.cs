using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Take.Party
{
    public class Settings
    {
        public string AccessToken { get; set; }
        public string PlayListId { get; set; }
        public int Timeout { get; set; }
        public string RefreshToken { get; set; }
        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
    }
}
