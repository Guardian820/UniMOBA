using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniMOBA_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer game_server = new GameServer();
            NetManager _netServer = new NetManager(game_server, 20, "unimoba");
            //server.ReuseAddress = true;
            if (!_netServer.Start(9050))
            {
                Console.WriteLine("Server start failed");
                Console.ReadKey();
                return;
            }
            game_server._netServer = _netServer;

            while (!Console.KeyAvailable)
            {
                _netServer.PollEvents();
                Thread.Sleep(15);
            }

            _netServer.Stop();
            Console.ReadKey();
            Console.WriteLine("ServStats:\n BytesReceived: {0}\n PacketsReceived: {1}\n BytesSent: {2}\n PacketsSent: {3}",
                _netServer.BytesReceived,
                _netServer.PacketsReceived,
                _netServer.BytesSent,
                _netServer.PacketsSent);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
