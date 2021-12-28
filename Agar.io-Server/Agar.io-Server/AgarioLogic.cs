using System;
using System.Numerics;

namespace Agar.io_Server
{
    class AgarioLogic
    {
        const int MaxFoodTimeout = 50;
        static int foodTimeout = 0;

        public static void Awake()
        {

        }

        public static void Update()
        {
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null && _client.player.isAlive)
                {
                    _client.player.Update();
                }
            }

            if (Server.clients.Count > 0)
            {
                if (foodTimeout == 0)
                {
                    Vector2 target;
                    do
                    {
                        Random rnd = new Random();
                        int valueX = rnd.Next(0, 800);
                        int valueY = rnd.Next(0, 800);
                        target = new Vector2(valueX, valueY);
                    } while (Server.food.Contains(target));

                    ServerSend.SpawnFood(target);
                    Server.food.Add(target);

                    foodTimeout++;
                }
                else if (foodTimeout == MaxFoodTimeout)
                    foodTimeout = 0;
                else
                    foodTimeout++;
            }

            ThreadManager.UpdateMain();
        }
    }
}
