using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Standard_UI
{
    public class SocketServer
    {
        public SocketServer(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }
        private Socket _socket = null;
        private string _ip = "";
        private int _port = 0;
        private bool _isListen = true;
        private void StartListen()
        {
            try
            {
                _socket.BeginAccept(asyncResult =>
                {
                    try
                    {
                        Socket newSocket = _socket.EndAccept(asyncResult);
                        if (_isListen)
                            StartListen();

                        SocketConnection newClient = new SocketConnection(newSocket, this)
                        {
                            HandleRecMsg = HandleRecMsg == null ? null : new Action<byte[], SocketConnection, SocketServer>(HandleRecMsg),
                            HandleClientClose = HandleClientClose == null ? null : new Action<SocketConnection, SocketServer>(HandleClientClose),
                            HandleSendMsg = HandleSendMsg == null ? null : new Action<byte[], SocketConnection, SocketServer>(HandleSendMsg),
                            HandleException = HandleException == null ? null : new Action<Exception>(HandleException)
                        };

                        newClient.StartRecMsg();
                        ClientList.AddLast(newClient);

                        HandleNewClientConnected?.Invoke(this, newClient);
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }


        public void StartServer()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(_ip);
                IPEndPoint endpoint = new IPEndPoint(address, _port);
                _socket.Bind(endpoint);
                _socket.Listen(int.MaxValue);
                StartListen();
                HandleServerStarted?.Invoke(this);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }

        public LinkedList<SocketConnection> ClientList { get; set; } = new LinkedList<SocketConnection>();

        public void CloseClient(SocketConnection theClient)
        {
            theClient.Close();
        }

        public Action<Exception> HandleException { get; set; }


        public Action<SocketServer> HandleServerStarted { get; set; }


        public Action<SocketServer, SocketConnection> HandleNewClientConnected { get; set; }


        public Action<SocketServer, SocketConnection> HandleCloseClient { get; set; }


        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg { get; set; }


        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }


        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }

    }


}
