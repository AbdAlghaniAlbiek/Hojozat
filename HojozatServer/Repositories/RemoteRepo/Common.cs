using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Repositories.RemoteRepo
{
    public sealed class Common
    {
        public static string URL { get => @"http://192.168.1.103:80/Hojozat/Server"; }
        public static int serverUserId { get; set; }
        public static string Token { get; set; }
    }
}
