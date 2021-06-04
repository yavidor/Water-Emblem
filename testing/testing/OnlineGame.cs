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
        public delegate void OnConnectionHandler();

        abstract class OnlineGame
        {
            protected BinaryReader reader;
            protected BinaryWriter writer;

            protected Thread thread;

            protected TcpClient client;

            protected int port;

            public Unit hostChar, joinChar;

            public event OnConnectionHandler OnConnection;

            #region funcs to overide
            protected abstract void InitChars();


            protected abstract void SocketThread();
            #endregion

            protected void InitChars1()
            {
            hostChar = new Unit();

            joinChar = new Unit();
            }
            protected void RaiseOnConnectionEvent()
            {

                OnConnection?.Invoke();
            }

            public void StartCommunication()
            {

            thread = new Thread(new ThreadStart(SocketThread))
            {
                IsBackground = true
            };
            thread.Start();
            }

            protected void ReadAndUpdateCharacter(Unit c)
            {
                c.X = reader.ReadInt32();
                c.Y = reader.ReadInt32();
            }

            protected void WriteCharacterData(Unit c)
            {
                writer.Write(c.X);
                writer.Write(c.Y);
            }



        }
    }
