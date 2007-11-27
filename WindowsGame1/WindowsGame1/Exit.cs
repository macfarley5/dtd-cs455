using System;
using System.Collections.Generic;
using System.Text;

namespace TD3d
{
    class Exit : Tile
    {
        public override bool isOccupied()
        {
            return false;
        }

        public override TileType getTileType()
        {
            return TileType.Exit;
        }

        public override void draw()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
