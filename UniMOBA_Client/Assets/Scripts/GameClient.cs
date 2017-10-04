using UnityEngine;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Linq;

public class GameClient : MonoBehaviour, INetEventListener
{
    public static GameClient instance
    {
        get; private set;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    NetManager _netClient;
    NetSerializer _netSerializer = new NetSerializer();
    string address;
    int port;

    void Start()
    {
        _netClient = new NetManager(this, "unimoba");
        _netClient.Start();
        _netClient.UpdateTime = 15;

        //_netSerializer.SubscribeReusable<StringArrayPacket, NetPeer>(OnStringArrayPacketReceived);

        Connect("localhost", 9050);
    }

    public void Connect(string address, int port)
    {
        this.address = address;
        this.port = port;
        _netClient.Connect(address, port);
    }

    void Update()
    {
        _netClient.PollEvents();

        var peer = _netClient.GetFirstPeer();
        if (peer != null && peer.ConnectionState == ConnectionState.Connected)
        {
            
        }
        else
        {
            _netClient.SendDiscoveryRequest(new byte[] { 1 }, port);
        }
    }

    void OnDestroy()
    {
        if (_netClient != null)
            _netClient.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("[CLIENT] We connected to " + peer.EndPoint);
    }

    public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
    {
        Debug.Log("[CLIENT] We received error " + socketErrorCode);
    }

    public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
    {
        _netSerializer.ReadAllPackets(reader, peer);
    }

    public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
    {
        if (messageType == UnconnectedMessageType.DiscoveryResponse && _netClient.PeersCount == 0)
        {
            Debug.Log("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
            _netClient.Connect(remoteEndPoint);
        }
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {

    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("[CLIENT] We disconnected because " + disconnectInfo.Reason);
    }

    //private void OnStringArrayPacketReceived(StringArrayPacket packet, NetPeer peer)
    //{
    //    packet.str_array.ToList().ForEach(x => Debug.Log(x));
    //}
}
