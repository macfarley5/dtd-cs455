using System;
using System.Collections.Generic;
using System.Text;

namespace TD3d
{
    class Wall : Tile
    {
        public override bool isOccupied()
        {
            return true;
        }

        public override TileType getTileType()
        {
            return TileType.Wall;
        }

        public override void draw()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
