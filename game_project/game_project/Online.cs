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
   

        abstract class Online
        {
            protected int port;
            protected string ip;
            protected Thread thread;
            protected int stam = 5;
            protected TcpClient client;
            #region private_func
            protected void startCommunication()
            {
                thread = new Thread(new ThreadStart(sockt));
                thread.IsBackground = true;
                thread.Start();
            }
            #endregion
            public abstract void sockt();


        }
    }