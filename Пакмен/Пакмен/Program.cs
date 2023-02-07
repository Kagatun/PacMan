using System;
using System.IO;

namespace Пакмен
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Random random = new Random();
            bool isAlive = true;
            bool isPlaying = true;
            int pacmanX;
            int pacmanY;
            int pacmanDirectionX = 0;
            int pacmanDirectionY = 1;
            int counterPositionY = 20;
            int victoryMessagePositionY = 21;
            int ghostX;
            int ghostY;
            int ghostDirectionX = 0;
            int ghostDirectionY = -1;
            int allDots = 0;
            int collectDots = 0;

            char[,] map = ReadMap("map0", out pacmanX, out pacmanY, out ghostX, out ghostY, ref allDots);

            DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(0, counterPositionY);
                Console.Write($"Собрано {allDots}/{collectDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    ChangeDirection(key, ref pacmanDirectionX, ref pacmanDirectionY);
                }

                if (map[pacmanX + pacmanDirectionX, pacmanY + pacmanDirectionY] != '#')
                {
                    Move(map, '@', ref pacmanX, ref pacmanY, pacmanDirectionX, pacmanDirectionY);

                    CollectDots(map, pacmanX, pacmanY, ref collectDots);
                }

                if (map[ghostX + ghostDirectionX, ghostY + ghostDirectionY] != '#')
                {
                    Move(map, '$', ref ghostX, ref ghostY, ghostDirectionX, ghostDirectionY);
                }
                else
                {
                    ChangeDirection(random, ref ghostDirectionX, ref ghostDirectionY);
                }

                System.Threading.Thread.Sleep(180);

                if (pacmanX == ghostX && pacmanY == ghostY)
                {
                    isAlive = false;
                }

                if (collectDots == allDots || !isAlive)
                {
                    isPlaying = false;
                }               
            }

            Console.SetCursorPosition(0, victoryMessagePositionY);
            if (collectDots == allDots)
            {
                Console.Write("Победа!");
            }
            else if (!isAlive)
            {
                Console.Write("Вы проиграли, Вас съели");
            }

        }

        static void Move(char[,] map, char symbol, ref int valueX, ref int valueY, int directionX, int directionY)
        {
            Console.SetCursorPosition(valueY, valueX);
            Console.Write(map[valueX, valueY]);

            valueX += directionX;
            valueY += directionY;

            Console.SetCursorPosition(valueY, valueX);
            Console.Write(symbol);

        }

        static void CollectDots(char[,] map, int valueX, int valueY, ref int collectDots)
        {
            if (map[valueX, valueY] == '.')
            {
                collectDots++;
                map[valueX, valueY] = ' ';
            }
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int directionX, ref int directionY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    directionX = -1;
                    directionY = 0;
                    break;

                case ConsoleKey.DownArrow:
                    directionX = 1;
                    directionY = 0;
                    break;

                case ConsoleKey.LeftArrow:
                    directionX = 0;
                    directionY = -1;
                    break;

                case ConsoleKey.RightArrow:
                    directionX = 0;
                    directionY = 1;
                    break;
            }
        }

        static void ChangeDirection(Random random, ref int directionX, ref int directionY)
        {
            int randomMinNumber = 1;
            int randomMaxNumber = 5;
            int randomDirection = random.Next(randomMinNumber, randomMaxNumber);

            switch (randomDirection)
            {
                case 1:
                    directionX = -1;
                    directionY = 0;
                    break;

                case 2:
                    directionX = 1;
                    directionY = 0;
                    break;

                case 3:
                    directionX = 0;
                    directionY = -1;
                    break;

                case 4:
                    directionX = 0;
                    directionY = 1;
                    break;
            }
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        static char[,] ReadMap(string mapName, out int pacmanX, out int pacmanY, out int ghostX, out int ghostY, ref int allDots)
        {
            pacmanX = 0;
            pacmanY = 0;
            ghostX = 0;
            ghostY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        pacmanX = i;
                        pacmanY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == '$')
                    {
                        ghostX = i;
                        ghostY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }

            return map;
        }
    }
}
