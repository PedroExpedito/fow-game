using FowGameLib;
using Godot;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FowGameClient
{
    public class Game : Spatial
    {
        private PlayerConnection _playerConnection;

        public override void _Ready()
        {
        }

        private int _packetCount;
        public override void _PhysicsProcess(float delta)
        {
            _playerConnection?.SendData(System.Text.Encoding.UTF8.GetBytes($"client _packetCount {_packetCount++}"));
        }

        public async void OnButton2_pressed()
        {
            var t = Task.Factory.StartNew(() =>
            {
                TcpClient client = new TcpClient("localhost", 32500);
                return new PlayerConnection(client);
            });

            _playerConnection = await t;
            _playerConnection.SendData(System.Text.Encoding.UTF8.GetBytes("Hi Server!"));
        }
    }
}
