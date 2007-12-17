using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace  TD3d
{
    class Entrance : Tile
    {
        public override bool isOccupied()
        {
            return false;
        }

        public override bool isTower()
        {
            return false;
        }

        public override TileType getTileType()
        {
            return TileType.ENTRANCE;
        }

        public override void updateState(float elapsedTime)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void draw(Matrix viewMatrix, Matrix projectionMatrix, bool showProjectile)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
