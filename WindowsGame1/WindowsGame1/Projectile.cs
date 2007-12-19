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
    public abstract class Projectile
    {
        protected Position pos;
        protected float rot;
        protected Position velocity;
        protected float speed = .005f;
        //protected float length = 2.5f;
        protected int damageDone;
        protected bool doesSplash = false;

        protected string modelAsset = "Content/bullet";
        protected float scale = .3f;
        protected Effect effect;
        protected Model model;
        protected ArrayList creeps;
        protected Creep target;

        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;

        protected bool isActive = true;

        public Position getPosition()
        {
            return pos;
        }
        public void setPosition(Position pos)
        {
            this.pos = pos;
        }

        public Position getVelocity()
        {
            return this.velocity;
        }

        public Creep getTarget()
        {
            return this.target;
        }

        public void updateState(float elapsedTime)
        {
            if (isActive)
            {
                Vector2 newVelocity = new Vector2(this.target.getVisualPosition().getX() - this.pos.getX(), this.target.getVisualPosition().getY() - this.pos.getY());
                newVelocity.Normalize();
                this.velocity = new Position(newVelocity.X, newVelocity.Y);
                float newX = this.pos.getX() + elapsedTime * this.velocity.getX() * speed;
                this.pos.setX(newX);
                float newY = this.pos.getY() + elapsedTime * this.velocity.getY() * speed;
                this.pos.setY(newY);

                rot = (float)Math.Atan2(newVelocity.Y, newVelocity.X);

                if (this.creeps.Count > 0)
                {
                    ArrayList hurtCreeps = new ArrayList();

                    foreach (Creep c in this.creeps)
                    {
                        Position visualPos = c.getVisualPosition();
                        float nowdist = Position.dist(this.pos, visualPos);
                        if (nowdist < .5)
                        {
                            hurtCreeps.Add(c);
                            isActive = false;
                        }
                    }
                    if (hurtCreeps.Count > 0){
                        if (this.doesSplash)
                        {
                            foreach (Creep c in this.creeps)
                            {
                                Position visualPos = c.getVisualPosition();
                                float nowdist = Position.dist(this.pos, visualPos);
                                if (nowdist < 3.5)
                                {
                                    c.injure(this.damageDone);
                                }
                            }
                        }
                        else
                        {
                            ((Creep)hurtCreeps[0]).injure(this.damageDone);
                        }
                    }
                }
            }
        }

        public abstract void draw(Matrix viewMatrix, Matrix projectionMatrix);

    }
}
