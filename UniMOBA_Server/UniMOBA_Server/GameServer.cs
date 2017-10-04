using System;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UniMOBA_Server
{
    public class GameServer : INetEventListener
    {
        public NetManager _netServer;
        NetSerializer _netSerializer = new NetSerializer();

        public void OnPeerConnected(NetPeer peer)
        {
            Console.WriteLine("[Server] Peer connected: " + peer.EndPoint);
            var peers = _netServer.GetPeers();
            foreach (var netPeer in peers)
            {
                Console.WriteLine("ConnectedPeersList: id={0}, ep={1}", netPeer.ConnectId, netPeer.EndPoint);
            }

            //peer.Send(_netSerializer.Serialize(new StringArrayPacket() { str_array = new string[] { "hello", "world" } }), SendOptions.ReliableOrdered);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine("[Server] Peer disconnected: " + peer.EndPoint + ", reason: " + disconnectInfo.Reason);
        }

        public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
        {
            Console.WriteLine("[Server] error: " + socketErrorCode);
        }

        public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
        {
            _netSerializer.ReadAllPackets(reader, peer);
        }

        public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
        {
            Console.WriteLine("[Server] ReceiveUnconnected: {0}", reader.GetString(100));
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }
    }
}