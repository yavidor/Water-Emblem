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


            hostChar = new Unit();

                joinChar = new Unit();
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
                    WriteCharacterData(hostChar);
                    ReadAndUpdateCharacter(joinChar);

                    Thread.Sleep(10);
                }

            }
        }
    }
