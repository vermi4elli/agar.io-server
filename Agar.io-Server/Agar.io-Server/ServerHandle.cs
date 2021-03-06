using System;
using System.Numerics;

namespace Agar.io_Server
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(_username);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }

            if (Server.clients[_fromClient].player.isAlive)
                Server.clients[_fromClient].player.SetInput(_inputs);
        }

        public static void EatFood(int _fromClient, Packet _packet)
        {
            Vector2 position = _packet.ReadVector2();

            if (Server.food.Contains(position))
            {
                Server.food.Remove(position);

                if (Server.clients[_fromClient].player.isAlive)
                    Server.clients[_fromClient].player.EatFood(position);
            }
        }

        public static void EatPlayer(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            bool areBothAlive = Server.clients[_clientIdCheck].player.isAlive
                && Server.clients[_fromClient].player.isAlive;

            if (!areBothAlive)
                return;

            if (Server.clients[_clientIdCheck].player.score > Server.clients[_fromClient].player.score)
                Server.clients[_fromClient].player.Die(_clientIdCheck);
            else
                Server.clients[_clientIdCheck].player.Die(_fromClient);
        }
    }
}
