using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

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

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
