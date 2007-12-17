using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace TD3d
{
<<<<<<< .mine
    abstract class Tower : Tile
=======
    public class Tower : Tile
>>>>>>> .r68
    {
        protected Creep target;
        protected float scale = .13f;
        protected float fireSpeed = 1000f;
        protected float fireCounter = 1f;
        protected float rot = 0;
        protected int range = 3;
        protected ArrayList projectiles = new ArrayList();
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;
        protected ArrayList creeps;

        public Tower()
        {
        }

        public void setTarget(Creep creep)
        {
            target = creep;
        }

        public Creep getTarget()
        {
            return target;
        }

        protected double getTurretAngle()
        {
            if (target != null)
            {
                double yOverX = (target.getPosition().getY() - getPosition().getY()) /
                                (target.getPosition().getX() - getPosition().getX());
                return Math.Atan(yOverX);
            }
            else return 0;
        }
        
        public override bool isOccupied()
        {
            return true;
        }

        public override bool isTower()
        {
            return true;
        }

        public override Tile.TileType getTileType()
        {
            return TileType.TOWER;
        }

        public override void updateState(float elapsedTime)
        {
        }

        public override void draw(Matrix vm, Matrix pm, bool showProjectile)
        {
        }
        
    }
}
