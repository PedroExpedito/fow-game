using Godot;
using System.Net.Sockets;
using System;
using Thread = System.Threading.Thread;

namespace FowGameLib
{
    public class PlayerConnection
    {
        private const int BufferSize = 1024;

        private Thread _thread;
        private TcpClient _tcpClient;
        private NetworkStream _clientStream;
        private bool _started = false;

        public PlayerConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _started = true;

            _clientStream = _tcpClient.GetStream();
            _thread = new Thread(ListenForPackets);
            _thread.Start();
        }

        private void ListenForPackets()
        {
            int bytesRead;
            byte[] data = new byte[BufferSize];

            while (_started)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = _clientStream.Read(data, 0, BufferSize);
                }
                catch (Exception ex)
                {
                    GD.Print("A socket error has occurred with the client socket " + ex.ToString());
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                GD.Print("ListenForPackets " + System.Text.Encoding.UTF8.GetString(data, 0, bytesRead));
            }
            _started = false;
            Disconnect();
        }

        public void SendData(byte[] data)
        {
            if (data.Length > BufferSize)
            {
                GD.PrintErr($"Data is bigger than the buffer with sizeof {data.Length}");
                return;
            }
            if (_clientStream.CanWrite)
            {
                _clientStream.Write(data, 0, data.Length);
                _clientStream.Flush();
            }
        }

        public void Disconnect()
        {
            GD.Print("Disconnect");
            if (_tcpClient == null)
            {
                return;
            }

            _tcpClient.Close();
        }
    }
}
