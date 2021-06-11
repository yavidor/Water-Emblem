using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testing
{

    class Host : OnlineGame
    {

        public Host(int port)
        {
            this.Port = port;


            InitChars();
            StartCommunication();
        }

        protected override void InitChars()
        {
            Units = Game1.Units;
        }

        protected override void SocketThread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Client = listener.AcceptTcpClient();
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

                WriteCharacterData(Units);
            }
            else
            {
                ReadAndUpdateCharacter(Units);
            }
        }

    }
}