using System;
using System.Collections;

/// <summary>
/// The Map class
/// </summary>
namespace TD3d
{
    public class Map
    {
        public Tile[,] layout; // keeps track of all the towers on the board
        public int width;
        public int height;
        public int depth;
        public ArrayList towers = new ArrayList();

        public Map()
        {
            init(50, 40);
        }

        public Map(int width, int depth)
        {
            init(width, depth);
        }

        public bool isOccupied(int x, int y)
        {
            bool retVal = false;
            if (this.layout[x, y] != null)
            {
                retVal = true;
            }
            return retVal;
        }

        public Tower getTower(int x, int y)
        {
            if (this.layout[x, y] != null && this.layout[x, y].isTower())
            {
                return (Tower)this.layout[x, y];
            }
            else return null;
        }

        /// <summary>
        /// Initializes the board. Every location is set to null.
        /// </summary>
        /// <param name="width">the width of the board in X</param>
        /// <param name="depth">the depth of the board in Z</param>
        private void init(int width, int depth)
        {
            this.width = width;
            this.height = depth;
            layout = new Tile[width, depth];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    layout[i, j] = null;
                }
            }                         
        }

        public bool canPlaceTower(Tile tower)
        {
            int x = (int)tower.getPosition().getX();
            int y = (int)tower.getPosition().getY();
            if (x < 0 || x >= width - 1 || y < 0 || y >= height - 1)
                return false;
            return layout[x, y] == null && layout[x + 1, y] == null && layout[x, y + 1] == null && layout[x + 1, y + 1] == null;
        }

        /// <summary>
        /// Places a tower on the map
        /// </summary>
        /// <param name="tower">the tower to be placed</param>
        /// <returns>true if the tower was successfully placed, or false if it couldn't be placed.</returns>
        public bool placeTower(Tile tower)
        {
            int x = (int)tower.getPosition().getX();
            int y = (int)tower.getPosition().getY();
            //can't go out of bounds
        if (x<0 || x>=width-1 || y<0 || y>=height-1)
                return false;
            //can't be placed on top of other towers
            if (layout[x, y] != null || layout[x+1, y] != null || layout[x, y+1] != null || layout[x+1, y+1] != null )
            {
                return false;
            }

            //once I can place it, do so and return true
            layout[x, y] = tower;
            layout[x + 1, y] = tower;
            layout[x, y + 1] = tower;
            layout[x+1, y + 1] = tower;
            this.towers.Add(tower);
            return true;
        }

        /// <summary>
        /// Removes the given tower according to location
        /// </summary>
        /// <param name="tower">the tower to be removed</param>
        public void removeTower(Tile tower)
        {
            // get the corrdinates of the tower
            int x = (int)tower.getPosition().getX();
            int y = (int)tower.getPosition().getY();

            // remove the tower by setting all locations to null
            layout[x, y] = null;
            layout[x + 1, y] = null;
            layout[x + 1, y + 1] = null;
            layout[x, y + 1] = null;

            this.towers.Remove(tower);
        }
    }
}