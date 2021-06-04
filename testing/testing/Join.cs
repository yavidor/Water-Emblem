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
    class Join : OnlineGame
    {
        string hostip;

        public Join(string hostip, int port)
        {
            this.port = port;
            this.hostip = hostip;
            InitChars();
            StartCommunication();

        }

        protected override void InitChars()
        {
            Units = Game1.Units;
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
                this.Update();
            }
            Thread.Sleep(10);
        }
        protected override void Update()
        {
            if (Game1.Turn == true)
            {
                Game1.Turn = false;
                ReadAndUpdateCharacter(Units);
                Game1.Turn = true;
            }
            else
            {
                WriteCharacterData(Units);
            }
        }
    }
}
