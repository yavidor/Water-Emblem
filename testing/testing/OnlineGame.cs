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

            public List<Unit> Units;

            public event OnConnectionHandler OnConnection;

            #region funcs to overide
            protected abstract void InitChars();


            protected abstract void SocketThread();
            protected abstract void Update();
            #endregion

            protected void InitChars1()
            {
            Units = Game1.Units;

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

            protected void ReadAndUpdateCharacter(List<Unit> units)
            {
                foreach(Unit unit in units)
            {
                Move move = new Move(unit, Game1.Grid[reader.ReadInt32(), reader.ReadInt32()], false);
                move.Execute();
                unit.Stats["HP"] = reader.ReadInt32();


            }

                Game1.Turn = reader.ReadBoolean();
            }

            public void WriteCharacterData(List<Unit> units)
            {
      
            foreach(Unit unit in units) {
                writer.Write(unit.X);
                writer.Write(unit.Y);
                writer.Write(unit.Stats["HP"]);


            }
            writer.Write(Game1.Turn);
            }



        }
    }
