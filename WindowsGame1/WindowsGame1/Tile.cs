using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.Text;

namespace TD3d
{
    public abstract class Tile
    {
        protected Effect effect;
        protected string modelAsset;
        protected Model model;
        protected string worldEffectString = "g_mWorld";
        protected string viewEffectString = "g_mView";
        protected string projectionEffectString = "g_mProj";

        public enum TileType { Tower, Wall, Exit, Entrance };
        public enum Player { p1, p2 };

        protected Position pos;
        Player owner;

        public Tile() { }

        public Tile(Position pos, Player owner)
        {
            this.pos = pos;
            this.owner = owner;
        }

        public Position getPosition()
        {
            return pos.Clone();
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
        public abstract void updateState(float elapsedTime);
        public abstract void draw( Matrix viewMatrix,Matrix projectionMatrix, bool showProjectile);
    }
}
