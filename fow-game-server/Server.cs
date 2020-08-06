using FowGameLib;
using Godot;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FowGameServer
{
    public class Server : Node
    {
        private TcpListener _server;
        private Task<TcpClient> _acceptTcpClientTask;

        private List<PlayerConnection> _playersConnection = new List<PlayerConnection>();

        public override void _Ready()
        {
            //GD.Print(new FowGameLib.FowGameLib1().HelloWorld());
            _server = new TcpListener(IPAddress.Any, 32500);
            _server.Start();

            _acceptTcpClientTask = _server.AcceptTcpClientAsync();
        }

        public override void _PhysicsProcess(float delta)
        {
        }

        public override void _Process(float delta)
        {
            if (_acceptTcpClientTask.IsCompleted)
            {
                var player = new PlayerConnection(_acceptTcpClientTask.Result);
                _playersConnection.Add(player);
                
                player.SendData(System.Text.Encoding.UTF8.GetBytes("Hi Player!"));
                _acceptTcpClientTask = _server.AcceptTcpClientAsync();
            }
        }
    }
}
