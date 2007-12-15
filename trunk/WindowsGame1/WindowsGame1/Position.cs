using System;
using System.Collections.Generic;
using System.Text;

namespace TD3d
{
    public class Position
    {
        float x;
        float y;

        public Position(float x, float y) 
        {
            this.x = x;
            this.y = y;
        }

        public float getX() { return x; }
        public float getY() { return y; }
        public void moveX(float x) { this.x += x; }
        public void moveY(float y) { this.y += y; }
        public void setX(float x)
        {
            this.x = x;
        }
        public void setY(float y)
        {
            this.y = y;
        }

        public Position Clone()
        {
            Position p = new Position(this.x, this.y);
            return p;
        }

        public static float dist(Position pos1, Position pos2)
        {
            float a = pos1.getX() - pos2.getX();
            float b = pos1.getY() - pos1.getY();
            return (float)Math.Sqrt(a * a + b * b);
        }
    }
}
