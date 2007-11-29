using System;
using System.IO;
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
    abstract class Creep
    {
        protected Position pos = new Position(0f, 0f);
        protected  int level = 0;
        protected int health = 80;
        protected float speed = .001f;
        protected int cash = 3;
        private float threshold = .1f;
        private int currentPathPlaceholder = 0;


        protected string worldEffectString = "";
        protected string viewEffectString = "";
        protected string projectionEffectString = "";
        protected Effect effect;
        protected ArrayList currentPath = new ArrayList();
        protected Model model;

        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;

        public Position getPosition()
        {
            return pos;
        }
        public void setPosition(Position pos)
        {
            this.pos = pos;
        }

        public long getHealth()
        {
            return this.health;
        }
        public double getSpeed()
        {
            return this.speed;
        }
        public int getCashValue()
        {
            return cash;
        }

        private void generateTestPath()
        {
            Random r = new Random((int)this.pos.getX());
            for (int i = 0; i < 10; i++)
            {
                currentPath.Add(new Position(r.Next(40), r.Next(40)));
            }
            this.currentPathPlaceholder = 0;
        }

        public void move(float timePassed)
        {
            if (timePassed > 0)
            {
                Position pos = this.pos;
                float oldX = pos.getX();
                if (this.currentPathPlaceholder<currentPath.Count )
                {
                    Position target = (Position)currentPath[this.currentPathPlaceholder];
                    float movex = target.getX() - pos.getX();
                    float movey = target.getY() - pos.getY();

                    if (Math.Abs(movex) > this.threshold / 2)
                    {
                        if (movex < 0)
                            this.pos.moveX(-timePassed * speed);
                        else
                            this.pos.moveX(timePassed * speed);
                    }

                    if (Math.Abs(movey) > this.threshold / 2)
                    {
                        if (movey < 0)
                            this.pos.moveY(-timePassed * speed);
                        else
                            this.pos.moveY(timePassed * speed);
                    }

                    if (Math.Abs(this.pos.getX() - target.getX()) < this.threshold && Math.Abs(this.pos.getY() - target.getY()) < this.threshold)
                    {
                        currentPathPlaceholder++;
                    }
                }
                else
                {
                    this.generateTestPath();
                }
            }
        }

        public abstract void draw(Matrix viewMatrix, Matrix projectionMatrix);
        
    
        
    }
}
