using System;

namespace Battleship
{
    class Player
    {
        public string name { get; set; }
        private static Board playerBoard;

        public Player()
        {
            playerBoard = new Board();
        }

        /*
         * Function description:
         *  Places ship on player's board.
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
            return playerBoard.placeShip(coord, size, direction);
        }

        /*
         * Function description:
         *  Places a strike on this player's ships.
         *  
         * Parameters:
         *  coord = The coordinate to strike.
         * 
         * Returns:
         *  Info, if there are errors and/or successes and if all ships have been hit.
         */
        public IDictionary<string, bool> attackThisPlayersShip(object coord)
        {
            return playerBoard.placeStrike(coord);
        }

        /*
         * Function description:
         *  Returns the strike coordinates.
         * 
         * Parameters:
         *  None.
         *  
         * Returns:
         *  The strike coordinates.
         */
        public List<Tuple<int, int>> strikesOnThisPlayersShips()
        {
            return playerBoard.getStrikeCoords();
        }

        /*
         * Function description:
         *  Returns the board represented as a string.
         * 
         * Parameters:
         *  censored = If the ships should be censored.
         * 
         * Returns:
         *  The board represented as a string.
         */
        public string getBoard(bool censored)
        {
            return playerBoard.getBoard(censored);
        }
    }
}
 
