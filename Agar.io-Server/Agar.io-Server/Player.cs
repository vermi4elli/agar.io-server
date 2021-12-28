using System.Numerics;

namespace Agar.io_Server
{
    class Player
    {
        public int id;
        public int score;
        public string username;

        public Vector2 position;

        private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
        private bool[] inputs;

        public Player(int _id, string _username, Vector2 _spawnPosition)
        {
            id = _id;
            username = _username;
            position = _spawnPosition;
            score = 0;

            inputs = new bool[4];
        }

        public void Update()
        {
            Vector2 _inputDirection = Vector2.Zero;
            if (inputs[0])
            {
                _inputDirection.Y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.Y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.X -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.X += 1;
            }

            Move(_inputDirection);
        }

        private void Move(Vector2 _inputDirection)
        {
            position += _inputDirection * moveSpeed;
            ServerSend.PlayerPosition(this);
        }

        public void SetInput(bool[] _inputs)
        {
            inputs = _inputs;
        }

        public void EatFood(Vector2 _position)
        {
            score += 10;
            ServerSend.FoodEaten(_position, this);
        }
    }
}
