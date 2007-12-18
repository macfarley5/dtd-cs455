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
    public abstract class Tower : Tile
    {
        protected Creep target;
        protected int level = 1;
        protected float scale = .13f;
        protected float fireSpeed = 2000f;
        protected float fireCounter = 1f;
        protected float rot = 0;
        protected int range = 4;
        protected ArrayList projectiles = new ArrayList();
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;
        protected ArrayList creeps;
        protected float alphaVal;

        public Tower()
        {
        }

        public int getLevel()
        {
            return level;
        }

        public int getRange()
        {
            return range;
        }

        public int getFireSpeed()
        {
            return (int)fireSpeed;
        }

        public int getAngle()
        {
            int angle = (int)(180f * rot / Math.PI);

            while (angle > 360)
                angle -= 360;

            while (angle < 0)
                angle += 360;

            return angle;
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
                return Math.Atan2(target.getPosition().getY() - getPosition().getY(), target.getPosition().getX() - getPosition().getX());
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

        public void setAlpha(float alph)
        {
            this.alphaVal = alph;
        }
        
    }
}
