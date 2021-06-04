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
            this.port = port;


            InitChars();
            StartCommunication();
        }

        protected override void InitChars()
        {
            Units = Game1.Units;
        }

        protected override void SocketThread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            client = listener.AcceptTcpClient();
            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());

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