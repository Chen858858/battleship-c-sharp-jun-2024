/*
 * Battleship made with C#, May 2024 version.
 * By Junchen Wang.
 *  Website: https://chen858858.github.io
 *  GitHub: https://github.com/Chen858858
 *  This project's repository: 
 * Created May 2024.
 */

using System;

namespace Battleship
{
    class Runner
    {
        /*
         * Function description:
         *  The gameplay.
         * 
         * Parameters:
         *  Nothing.
         * 
         * Returns:
         *  Nothing.
         */
        static void Main(string[] args)
        {
            Player humanPlayer = new Player();
            humanPlayer.isRobot = false;
            Player robotPlayer = new Player();
            robotPlayer.isRobot = true;
            List<string> robotNames = new List<string>() {
                "The Machine",
                "Mr. Roboto",
                "Hal 9000",
                "Robbie",
                "Elvex"
            };
            List <Tuple<string, int>> shipTypes = new List<Tuple<string, int>> {
                new Tuple<string, int>("Submarine", 3),
                new Tuple<string, int>("Carrier", 5),
                new Tuple<string, int>("Patrol Boat", 2),
                new Tuple<string, int>("Destroyer", 3),
                new Tuple<string, int>("Battleship", 4)
            };
            List<string> directions = new List<string>()
            {
                "v",
                "h"
            };
            List<Tuple<int, int>> robotTarget = new List<Tuple<int, int>>();
            IDictionary<string, bool> robotInitTargetSorrounding = new Dictionary<string, bool>
            {
                {"t", false},
                {"r", false},
                {"b", false },
                {"l", false}
            };
            string robotTargetDirection = "u";
            IDictionary<string, bool> robotTargetBoundaries = new Dictionary<string, bool>
            {
                {"tl", false},
                {"br", false}
            };

            // Human inputs name.
            Console.WriteLine("What is your name?");
            humanPlayer.name = Console.ReadLine();
            Console.WriteLine("Hello {0}.\n", humanPlayer.name);

            // Generate robot name.
            Random rand1 = new Random();
            robotPlayer.name = robotNames[rand1.Next(robotNames.Count)];
            Console.WriteLine("This game, you are playing against {0}.\n", robotPlayer.name);

            // Human places ships.
            Console.WriteLine("{0}, please place your ships.\n", humanPlayer.name);
            foreach(Tuple<string, int> shipType in shipTypes)
            {
                Console.WriteLine("Place your {0}, length of {1}.", shipType.Item1, shipType.Item2);
                while(true){
                    Console.WriteLine("Enter the coordinate (x, y), separated by a comma, no parentheses, x & y = 0 to 9:");
                    string[] coordinateStringSplit = Console.ReadLine().Split(",");
                    Console.WriteLine("Enter the direction, v = vertical, h = horizontal:");
                    string direction = Console.ReadLine();
                    IDictionary<string, bool> placeShipInfo = humanPlayer.placeShip(
                        coordinateStringSplit.Count() == 2 ?
                            new Tuple<string, string>(coordinateStringSplit[0], coordinateStringSplit[1])
                            :
                            false,
                        shipType.Item2,
                        direction
                        );
                    if (placeShipInfo["shipPlaced"])
                    {
                        Console.WriteLine("{0} placed.", shipType.Item1);
                        break;
                    }
                    if (!placeShipInfo["validCoord"])
                    {
                        Console.WriteLine("Error: Invalid coordinate.");
                    }
                    else if (!placeShipInfo["validDirection"])
                    {
                        Console.WriteLine("Error: Invalid direction.");
                    }
                    else if (!placeShipInfo["fitsOnBoard"])
                    {
                        Console.WriteLine("Error: Ship does not fit on board.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Ship conflicts with an already placed ship.");
                    }
                    Console.WriteLine("\n");
                }
                Console.WriteLine("\nHere is what your board currently looks like:\n{0}", humanPlayer.getBoard(false));
            }
            Console.WriteLine("You have placed all of your 5 ships. This is what your board looks like:");
            Console.WriteLine(humanPlayer.getBoard(false));

            // Robot places ships.
            Console.WriteLine("Now {0} is placing its ships.", robotPlayer.name);
            foreach(Tuple<string, int> shipType in shipTypes)
            {
                Random rand2 = new Random();
                while (true)
                {
                    int x = rand2.Next(0, 10);
                    int y = rand2.Next(0, 10);
                    string direction = directions[rand2.Next(0, 2)];
                    IDictionary<string, bool> placeShipInfo = robotPlayer.placeShip(
                        (object)new Tuple<string, string>(x.ToString(), y.ToString()),
                        shipType.Item2,
                        direction);
                    if (placeShipInfo["shipPlaced"])
                    {
                        break;
                    }
                }
                Console.WriteLine("{0} has placed its {1}.", robotPlayer.name, shipType.Item1);
            }
            Console.WriteLine("{0} has placed all of its ships.", robotPlayer.name);

            // Strike each others ships.
            while(true)
            {
                // Human place strike.
                Console.WriteLine("{0}, it's your turn to place a strike.", humanPlayer.name);
                Console.WriteLine("What {0}'s board looks like:", robotPlayer.name);
                Console.WriteLine(robotPlayer.getBoard(true));

                while (true)
                {
                    Console.WriteLine("Enter the coordinate you want to strike (x, y), separated by a comma, no parentheses, x & y = 0 to 9:");
                    string[] coordinateStringSplit = Console.ReadLine().Split(",");
                    IDictionary<string, bool> attackInfo = robotPlayer.attackThisPlayersShip(coordinateStringSplit.Count() == 2 ?
                        (object)new Tuple<string, string>(coordinateStringSplit[0], coordinateStringSplit[1])
                        :
                        (object)false);
                    if (attackInfo["strikePlaced"])
                    {
                        Console.WriteLine("The strike was placed, {0}",
                            attackInfo["isHit"] ?
                                "and you've hit a ship!"
                                :
                                "but you didn't hit a ship."
                            );
                        if (attackInfo["allShipsHit"])
                        {
                            endGame(humanPlayer);
                        }
                        break;
                    }
                    if (!attackInfo["validCoord"])
                    {
                        Console.WriteLine("Error: Invalid coordinate.");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Error: Coordinate already struck.");
                        continue;
                    }
                }

                // Robot place strike.
                while(true)
                {
                    // Not done.
                    break;
                }
            }
        }

        static void endGame(Player winner)
        {

        }

        static void printDictionaryStringBool(IDictionary<string, bool> dict)
        {
            foreach(KeyValuePair<string, bool> kvp in dict)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
        }
    }
}