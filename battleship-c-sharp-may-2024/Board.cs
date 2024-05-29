using System;

namespace Battleship
{
    class Board
    {
        private List<Tuple<int, int>> shipCoords = new List<Tuple<int, int>>();
        private List<Tuple<int, int>> strikeCoords = new List<Tuple<int, int>>();
        private List<Tuple<int, int>> hitCoords = new List<Tuple<int, int>>();

        /*
         * Function description:
         *  Returns the board represented as a string.
         * 
         * Parameters:
         *  censored = If the ships should be censored.
         * 
         * Returns:
         *  The board represented as a string.
         * 
         * Board key:
         *  ~ = Water.
         *  $ = Ship.
         *  X = Strike.
         *  * = Hit.
         */
        public string getBoard(bool censored)
        {
            string boardString = "   ";
            for(int x = 0; x < 10; x++)
            {
                boardString += $" {x}";
            }
            boardString += "\n";
            for(int y = 0; y < 10; y++)
            {
                boardString += $" {y}  ";
                for(int x = 0; x < 10; x++)
                {
                    Tuple<int, int> coord = new Tuple<int, int>(x, y);
                    if (hitCoords.Contains(coord))
                    {
                        boardString += "* ";
                        continue;
                    }
                    if (strikeCoords.Contains(coord))
                    {
                        boardString += "X ";
                        continue;
                    }
                    if (shipCoords.Contains(coord) && !censored)
                    {
                        boardString += "$ ";
                        continue;
                    }
                    boardString += "~ ";
                }
                boardString += "\n";
            }
            return boardString;
        }

        /*
         * Function description:
         *  Places a ship on the board, if valid.
         * 
         * Parameters:
         *  coord = If horizontal, leftmost coordinate. If vertical, highest coordinate.
         *  size = Length of ship.
         *  direction = Horizontal or vertical.
         * 
         * Returns:
         *  Info, if there are errors and/or successes.
         */
        public IDictionary<string, bool> placeShip(
            object coord,
            int size,
            string direction)
        {
            IDictionary<string, bool> info = new Dictionary<string, bool>();
            info.Add("validCoord", false);
            info.Add("validDirection", false);
            info.Add("fitsOnBoard", false);
            info.Add("noConflicts", false);
            info.Add("shipPlaced", false);

            // Checks if the coordinate is valid.
            if(!checkCoord(coord))
            {
                return info;
            }
            Tuple<string, string> coordTuple = (Tuple<string, string>)coord;
            (string x, string y) = coordTuple;
            info["validCoord"] = true;
            int xNum = int.Parse(x);
            int yNum = int.Parse(y);

            // Checks if the direction is valid.
            direction.ToLower();
            if(direction != "v" && direction != "h")
            {
                return info;
            }
            info["validDirection"] = true;

            // Checks if the ship fits on the board.
            if(
                (direction == "v" && yNum + size - 1 > 9) ||
                (direction == "h" && xNum + size - 1 > 9))
            {
                return info;
            }
            info["fitsOnBoard"] = true;

            // Checks if the ship doesn't conflict with ships already on the board.
            List<Tuple<int, int>> thisShipCoords = new List<Tuple<int, int>>();
            for(int offset = 0; offset < size; offset++)
            {
                thisShipCoords.Add(
                    Tuple.Create(
                        xNum + (direction == "h" ? offset : 0),
                        yNum + (direction == "v" ? offset : 0)
                    ));
            }
            foreach(Tuple<int, int> shipCoord in thisShipCoords)
            {
                if(shipCoords.Contains(shipCoord))
                {
                    return info;
                }
            }
            info["noConflicts"] = true;

            // Place the ship.
            shipCoords.AddRange(thisShipCoords);
            info["shipPlaced"] = true;
            return info;
        }

        /*
         * Function description:
         *  Places a strike on the board, if valid.
         * 
         * Parameters:
         *  coord = The coordinate to strike.
         * 
         * Returns:
         *  Info, if there are errors and/or successes and if all ships have been hit.
         */
        public IDictionary<string, bool> placeStrike(object coord)
        {
            IDictionary<string, bool> info = new Dictionary<string, bool>();
            info.Add("validCoord", false);
            info.Add("availableCoord", false);
            info.Add("strikePlaced", false);
            info.Add("isHit", false);
            info.Add("allShipsHit", false);

            // Checks if the coordinate is valid.
            if(!checkCoord(coord))
            {
                return info;
            }
            info["validCoord"] = true;
            Tuple<string, string> coordTuple = (Tuple<string, string>)coord;
            (string x, string y) = coordTuple;
            int xNum = int.Parse(x);
            int yNum = int.Parse(y);

            // Checks if the coordinate has been already struck.
            if(strikeCoords.Contains(Tuple.Create(xNum, yNum)))
            {
                return info;
            }
            info["availableCoord"] = true;

            // Place the strike.
            strikeCoords.Add(Tuple.Create(xNum, yNum));
            info["strikePlaced"] = true;

            // Checks if all ships have been hit, thus ending the game.
            if(hitCoords.Count == shipCoords.Count)
            {
                info["allShipsHit"] = true;
            }

            return info;
        }

        /*
         * Function description:
         *  Returns the strike coordinates.
         * 
         * Parameters:
         *  Nothing.
         * Returns:
         *  The strike coordinates.
         */
        public List<Tuple<int, int>> getStrikeCoords()
        {
            return strikeCoords;
        }

        /*
         * Function description:
         *  Checks if the coordinate is valid.
         *  
         * Parameters:
         *  coord = The coordinate to check.
         *  
         * Returns:
         *  If the coordinate is valid.
         */
        private static bool checkCoord(object coord)
        {
            // Checks if the coordinate is a tuple of (string, string).
            if (coord.GetType() != typeof(Tuple<string, string>))
            {
                return false;
            }
            Tuple<string, string> coordTuple = (Tuple<string, string>)coord;
            (string x, string y) = coordTuple;

            // Checks if the tuple elements are numbers.
            int xNum;
            int yNum;
            if (!int.TryParse(x, out xNum) || !int.TryParse(y, out yNum))
            {
                return false;
            }

            // Checks if x and y are between 0-9.
            if (xNum < 0 || xNum > 9 || yNum < 0 || yNum > 9)
            {
                return false;
            }
            return true;
        }
    }
}