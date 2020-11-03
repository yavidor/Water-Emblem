using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace game_project
{
    class Join : Online
    {

        public Join(int port, String ip)
        {
            this.port = port;
            this.ip = ip;

        }
        public override void sockt()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Any, port);
            Game1.msg = "connecting to server";
            while (true)
            {

            }
        }
    }
}
