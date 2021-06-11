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
            this.Port = port;
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
            Client = new TcpClient();
            Client.Connect(hostip, Port);
            Reader = new BinaryReader(Client.GetStream());
            Writer = new BinaryWriter(Client.GetStream());


            RaiseOnConnectionEvent();

            while (true)
            {
                this.Update();
                Thread.Sleep(10);
            }
        }
        protected override void Update()
        {
            if (Game1.Turn == true)
            {
                ReadAndUpdateCharacter(Units);
            }
            else
            {
                WriteCharacterData(Units);
            }
        }
    }
}
