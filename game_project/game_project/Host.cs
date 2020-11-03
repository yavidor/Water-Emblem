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
    class Host : Online
    {

        public Host(int port)
        {
            this.port = port;
            startCommunication();

        }

        public override void sockt()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            client = listener.AcceptTcpClient();
            int stam = 5;
            stam++;
            Game1.msg = stam.ToString();
            while (true)
            {
            }

        }
    }
}
