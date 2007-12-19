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
        protected float fireSpeed;
        protected float fireCounter = 1f;
        protected float rot = 0;
        protected int range;
        protected int cost;
        protected int damage;
        protected enum ProjectileType { BULLET, LASER };
        protected ProjectileType projType = ProjectileType.BULLET;
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

        public void incrementLevel()
        {
            if (level < 5)
                level++;
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

        public int getCost()
        {
            return cost;
        }

        public int getDamage()
        {
            return damage;
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
            this.fireCounter -= elapsedTime;
            if (this.fireCounter < 0)
            {
                //Fire!
                if (this.creeps.Count > 0)
                {
                    Position bestPos = ((Creep)creeps[0]).getVisualPosition();
                    Position myVisPos = this.getPosition();
                    myVisPos.setX(myVisPos.getX() + .5f);
                    myVisPos.setY(myVisPos.getY() + .5f);
                    float bestDist = 100000f;

                    // see if target is out of range. if so, set new target
                    if (this.target != null && (this.target.getHealth() <= 0 || Position.dist(this.target.getVisualPosition(), myVisPos) > range)) this.target = null;
                    if (this.target != null)
                    {
                        if (Position.dist(target.getVisualPosition(), myVisPos) > this.range)
                        {
                            this.target = null;
                            foreach (Creep creep in this.creeps)
                            {
                                float nowdist = Position.dist(myVisPos, creep.getVisualPosition());
                                if (nowdist < bestDist && nowdist < this.range)
                                {
                                    bestDist = nowdist;
                                    this.target = creep;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Creep creep in this.creeps)
                        {
                            float nowdist = Position.dist(myVisPos, creep.getVisualPosition());
                            if (nowdist < bestDist && nowdist < this.range)
                            {
                                bestDist = nowdist;
                                this.target = creep;
                            }
                        }

                        rot += elapsedTime / 2500;
                    }
                    if (this.target != null && Position.dist(this.target.getVisualPosition(), myVisPos) < this.range)
                    {
                        Vector2 velocity = new Vector2(bestPos.getX() - myVisPos.getX(), bestPos.getY() - myVisPos.getY());
                        velocity.Normalize();
                        Vector2 iniPos = new Vector2(myVisPos.getX(), myVisPos.getY()) + velocity;

                        if (this.getTileType() != TileType.FASTTOWER)
                        {
                            System.Console.WriteLine("FASTTOWER");
                            rot = (float)Math.Atan2(velocity.Y, velocity.X);
                        }

                        this.fireCounter = this.fireSpeed;
                        this.projectiles.Add(new ProjectileLaser(new Position(iniPos.X, iniPos.Y),
                            new Position(velocity.X, velocity.Y), this.creeps, this.graphics, this.content, this.device, this.target, this.damage));
                    }
                }
            }
            else
            {

            }

            for (int i = 0; i < this.projectiles.Count; i++)// (Projectile p in this.projectiles)
            {
                if (Position.dist(((Projectile)this.projectiles[i]).getPosition(), ((Projectile)this.projectiles[i]).getTarget().getVisualPosition()) < .05)
                {
                    this.projectiles.RemoveAt(i--);
                }
                else
                {
                    ((Projectile)this.projectiles[i]).updateState(elapsedTime);
                }
            }

            if (target != null)
            {
                if (this.getTileType() == TileType.FASTTOWER)
                {
                    System.Console.WriteLine("FASTTOWER");
                    rot += .005f * elapsedTime;
                }
                else this.rot = (float)(Math.Atan2(target.getVisualPosition().getY() - this.pos.getY() - .5, target.getVisualPosition().getX() - this.pos.getX() - .5));
            }
            else
            {
                //this.rot += .001f * elapsedTime;
            }
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
