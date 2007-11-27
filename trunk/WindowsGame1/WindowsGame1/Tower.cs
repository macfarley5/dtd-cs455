using System;
using System.Collections.Generic;
using System.Text;

namespace TD3d
{
    class Tower : Tile
    {
        Creep target;

        public Tower()
        {
            target = null;
        }

        public void setTarget(Creep creep)
        {
            target = creep;
        }

        public Creep getTarget()
        {
            return target;
        }

        private double getTurretAngle()
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

        public override Tile.TileType getTileType()
        {
            return TileType.Tower;
        }

        public override void draw()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
