using System;

/// <summary>
/// Summary description for Node
/// </summary>
namespace TD3d
{
    public class Node
    {
        public int x;
        public int z;
        private Position myPos;
        private int costToGo;
        private int cost;
        private Node parent;
        private int totalCost;

        public Node()
        {
            x = 0;
            z = 0;
            myPos = new Position(x, z);
            cost = 0;
            costToGo = 0;
            totalCost = 0;
            parent = null;
        }

        public Node(int inX, int inZ, Node inParent)
        {
            x = inX;
            z = inZ;
            myPos = new Position(x, z);
            cost = 0;
            costToGo = 0;
            totalCost = 0;
            parent = inParent;
        }

        public Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public int CostToGo
        {
            get { return costToGo; }
            set { costToGo = value; }
        }

        public int TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }
        }

        public void setPath(int inX, int inY)
        {
            myPos.setX(inX);
            myPos.setY(inY);
        }

        public Position getPosition()
        {
            return myPos;
        }
    }
}