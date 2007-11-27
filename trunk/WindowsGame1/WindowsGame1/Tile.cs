using System;
using System.Collections.Generic;
using System.Text;

namespace TD3d
{
    public abstract class Tile
    {
        public enum TileType { Tower, Wall, Exit, Entrance };
        public enum Player { p1, p2 };

        Position pos;
        Player owner;

        public Tile() { }

        public Tile(Position pos, Player owner)
        {
            this.pos = pos;
            this.owner = owner;
        }

        public Position getPosition()
        {
            return pos;
        }

        public void setPosition(int x, int y)
        {
            Position p = new Position((float)x, (float)y);
            this.pos = p;
        }

        public Player getOwner()
        {
            return owner;
        }

        public abstract bool isOccupied();
        public abstract TileType getTileType();
        public abstract void draw();
    }
}
