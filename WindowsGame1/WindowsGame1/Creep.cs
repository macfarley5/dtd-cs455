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
    public abstract class Creep
    {
        protected Position pos = new Position(0f, 0f);
        protected  int level = 0;
        protected int health = 80;
        protected float speed = .001f;
        protected int cash = 3;
        private float threshold = .1f;
        private int currentPathPlaceholder = 0;
        protected PathPlanner myPlanner;

        protected string worldEffectString = "";
        protected string viewEffectString = "";
        protected string projectionEffectString = "";
        protected Effect effect;
        protected ArrayList currentPath;
        protected Model model;

        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        protected GraphicsDevice device;

        protected enum States { NOTHING,DAMAGED };
        protected float currentStateCountdown; //ms of the current state/effect remaining
        protected States currentState = States.NOTHING;

        public void setPlanner(PathPlanner planner)
        {
            myPlanner = planner;
        }

        public bool hasPath()
        {
            myPlanner.StartX = (int)pos.getX();
            myPlanner.StartZ = (int)pos.getY();
            return myPlanner.isPath();
        }

        public void setNewPath()
        {
            currentPath = myPlanner.getPath();
            this.currentPathPlaceholder = 1;
        }

        public Position getVisualPosition()
        {
            //shifted by -.5 to show where it really appears
            Position visualPos = this.getPosition();
            visualPos.setX(visualPos.getX());
            visualPos.setY(visualPos.getY());
            return visualPos;
        }

        public Position getPosition()
        {
            return pos.Clone();

        }
        public void setPosition(Position pos)
        {
            this.pos = pos;
        }
        public void setPath(ArrayList inPath)
        {
            currentPath = inPath;
        }

        public long getHealth()
        {
            return this.health;
        }

        public void injure(int amount)
        {
            this.health -= amount;
            if (this.health < 0)
            {
                this.health = 0;
            }
            currentStateCountdown = 500f;
            currentState = States.DAMAGED;
            //Console.Out.WriteLine("**************DAMAGES*");
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
                //System.Console.WriteLine(this.currentPath.Count);
                if (currentPathPlaceholder < this.currentPath.Count )
                {
                    Node targetNode = (Node)currentPath[this.currentPathPlaceholder];
                    Position target = targetNode.getPosition();
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
                    //System.Console.WriteLine("Ran out of Path draw  me no more");
                    //this.generateTestPath();
                }
            }
        }

        public virtual void updateState(float elapsedTime)
        {

        }

        public abstract void draw(Matrix viewMatrix, Matrix projectionMatrix);
        
    
        
    }
}
