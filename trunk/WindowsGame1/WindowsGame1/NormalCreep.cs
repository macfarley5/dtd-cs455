using System;
using System.Collections.Generic;
using System.Text;

namespace TD3d
{
    class NormalCreep : Creep
    {

        public NormalCreep(Position pos, int level)
        {
            this.cash = 1;
            this.speed = 1;
            this.pos = pos;
            this.level = level;
        }

        /*public override Position getPosition() { return pos; }
        public override void setPosition(Position pos) { this.pos = pos; }
        public override long getHealth() { return health * level; }
        public override int getSpeed() { return speed; }
        public override int getCashValue() { return cash * level; }*/
    }
}
