using System;
using System.Net.Sockets;
using System.Text;

namespace Standard_UI
{

    public class SocketConnection
    {

        public SocketConnection(Socket socket, SocketServer server)
        {
            _socket = socket;
            _server = server;
        }

        private readonly Socket _socket;
        private bool _isRec = true;
        private SocketServer _server = null;
        private bool IsSocketConnected()
        {
            bool part1 = _socket.Poll(1000, SelectMode.SelectRead);
            bool part2 = (_socket.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

        public void StartRecMsg()
        {
            try
            {
                byte[] container = new byte[1024 * 1024 * 2];
                _socket.BeginReceive(container, 0, container.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndReceive(asyncResult);

                        if (length > 0 && _isRec && IsSocketConnected())
                            StartRecMsg();

                        if (length > 0)
                        {
                            byte[] recBytes = new byte[length];
                            Array.Copy(container, 0, recBytes, 0, length);

                            HandleRecMsg?.Invoke(recBytes, this, _server);
                        }
                        else
                            Close();
                    }
                    catch (Exception ex)
                    {
                        HandleException?.Invoke(ex);
                        Close();
                    }
                }, null);
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
                Close();
            }
        }

        public void Send(byte[] bytes)
        {
            try
            {
                _socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = _socket.EndSend(asyncResult);
                        HandleSendMsg?.Invoke(bytes, this, _server);
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

        public void Send(string msgStr)
        {
            Send(Encoding.UTF8.GetBytes(msgStr));
        }

        public void Send(string msgStr, Encoding encoding)
        {
            Send(encoding.GetBytes(msgStr));
        }

        public object Property { get; set; }


        public void Close()
        {
            try
            {
                _isRec = false;
                _socket.Disconnect(false);
                _server.ClientList.Remove(this);
                HandleClientClose?.Invoke(this, _server);
                _socket.Close();
                _socket.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                HandleException?.Invoke(ex);
            }
        }


        public Action<byte[], SocketConnection, SocketServer> HandleRecMsg { get; set; }


        public Action<byte[], SocketConnection, SocketServer> HandleSendMsg { get; set; }


        public Action<SocketConnection, SocketServer> HandleClientClose { get; set; }

        public Action<Exception> HandleException { get; set; }


    }
}
