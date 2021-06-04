using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testing
{
    class JoinOnlineGame : OnlineGame
    {
        string hostip;

        public JoinOnlineGame(string hostip, int port)
        {
            this.port = port;
            this.hostip = hostip;
            InitChars();
            StartCommunication();

        }

        protected override void InitChars()
        {
            hostChar = new Unit();

            joinChar = new Unit();
        }

        protected override void SocketThread()
        {
            client = new TcpClient();
            client.Connect(hostip, port);

            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());

            RaiseOnConnectionEvent();

            while (true)
            {
                ReadAndUpdateCharacter(hostChar);
                WriteCharacterData(joinChar);

                Thread.Sleep(10);
            }
        }
    }
}
