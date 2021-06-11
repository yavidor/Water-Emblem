using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected BinaryReader Reader;
        protected BinaryWriter Writer;

        protected Thread thread;

        protected TcpClient Client;

        protected int Port;

        public List<Unit> Units;

        public event OnConnectionHandler OnConnection;

        #region funcs to overide
        protected abstract void InitChars();


        protected abstract void SocketThread();
        protected abstract void Update();
        #endregion

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

        public void ReadAndUpdateCharacter(List<Unit> Units)
        {
            foreach (Unit unit in Units)
            {
                Move move = new Move(unit, Game1.Grid[Reader.ReadInt32(), Reader.ReadInt32()], false);
                move.Execute();
                unit.Stats["HP"] = Reader.ReadInt32();


            }

            Game1.Chosen = Game1.Grid[Reader.ReadInt32(), Reader.ReadInt32()];
            Game1.Turn = Reader.ReadBoolean();
        }

        public void WriteCharacterData(List<Unit> Units)
        {
            foreach (Unit unit in Units)
            {
                Writer.Write(unit.X);
                Writer.Write(unit.Y);
                Writer.Write(unit.Stats["HP"]);
            }
            Writer.Write(Game1.Chosen.X);
            Writer.Write(Game1.Chosen.Y);
            Writer.Write(Game1.Turn);
        }
        public void Stop()
        {
            Writer.Flush();
        }
    }
}