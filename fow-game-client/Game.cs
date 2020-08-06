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

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            //GD.Print(new FowGameLib.FowGameLib1().HelloWorld());
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
