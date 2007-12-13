using System;
using System.Collections;
//using System.Math;

/// <summary>
/// PathPlanner takes the demensions of the map and a starting and ending location and returns the 
/// best path between those locations.
/// </summary>

namespace TD3d
{
    public class PathPlanner
    {
        /// <summary>
        /// Custom comparer for the SortedList Fringe
        /// </summary>
        /// 
        
        public class nodeComparer : IComparer
        {
            int System.Collections.IComparer.Compare(Object firstv, Object secondv)
            {
                if (firstv is Node && secondv is Node)
                {
                    Node first = (Node)firstv;
                    Node second = (Node)secondv;
                    int retVal = 0;
                    if (first.TotalCost > second.TotalCost)
                    {
                        retVal = 1;
                    }
                    else if (first.TotalCost == second.TotalCost)
                    {
                        retVal = 0;
                    }
                    else if (first.TotalCost < second.TotalCost)
                    {
                        retVal = -1;
                    }
                    return retVal;
                }
                else
                {
                    return 0;
                }
            }
        }

        private ArrayList path;
        private Queue visited;
        private int startX;
        private int startZ;
        private int endZ;
        private int endX;
        private int depth;
        private int width;
        private Map myMap;

        public PathPlanner(int inWidth, int inDepth, int inStartX, int inStartZ, int inEndX, int inEndZ, Map inMap)
        {
            path = new ArrayList();
            visited = new Queue();
            startX = inStartX;
            startZ = inStartZ;
            endX = inEndX;
            endZ = inEndZ;
            width = inWidth;
            depth = inDepth;
            myMap = inMap;
        }

        private bool canMove(Node inNode, string dir)
        {
            bool moveAllowed = false;
            if ((width >= inNode.X && inNode.X >= 0) && (depth >= inNode.Z && inNode.Z >= 0))
            {
                moveAllowed = true;
                switch (dir)
                {
                    case "up":
                        if (inNode.Z + 1 >= depth)
                        {
                            moveAllowed = false;
                        }
                        else if (myMap.isOccupied(inNode.X, inNode.Z + 1))
                        {
                            moveAllowed = false;
                        }
                        break;
                    case "down":
                        if (inNode.Z - 1 < 0)
                        {
                            moveAllowed = false;
                        }
                        else if (myMap.isOccupied(inNode.X, inNode.Z - 1))
                        {
                            moveAllowed = false;
                        }
                        break;
                    case "left":
                        if (inNode.x - 1 < 0)
                        {
                            moveAllowed = false;
                        }
                        else if (myMap.isOccupied(inNode.X - 1, inNode.Z))
                        {
                            moveAllowed = false;
                        }
                        break;
                    case "right":
                        if (inNode.X + 1 >= width)
                        {
                           moveAllowed = false;
                        }
                        else if (myMap.isOccupied(inNode.X + 1, inNode.Z))
                        {
                            moveAllowed = false;
                        }
                        break;
                }
            }
            return moveAllowed;
        }

        public bool beenVisited(Node inNode)
        {
            bool retVal = false;
            if (visited.Count != 0)
            {
                foreach (Node tempy in visited)
                {
                    if (tempy.X == inNode.X && tempy.Z == inNode.Z)
                    {
                        retVal = true;
                    }
                }
                //System.Console.WriteLine("check visited for " + inNode.X + ", " + inNode.Z);
            }
            return retVal;
        }

        private ArrayList findPath()
        {
            IComparer myComparison = new nodeComparer();
            ArrayList fringe = new ArrayList();
            Node currentNode = new Node(startX, startZ, null);
            visited.Enqueue(currentNode);
            Node goalNode = new Node(endX, endZ, null);
            
            //System.Console.WriteLine("goal node = " + endX + ", " + endZ);
            //start at the beginning and go to the end
            bool blocked = false;

            while (!(currentNode.X == goalNode.X && currentNode.Z == goalNode.Z))
            {
                int newX = currentNode.X;
                int newZ = currentNode.Z;
                if (canMove(currentNode, "up"))
                {
                    Node temp = new Node(newX, newZ + 1, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - (newZ + 1));
                        temp.CostToGo = Math.Abs(endX - newX);
                        temp.TotalCost = temp.Cost + temp.CostToGo;
                        //visited.Enqueue(temp);
                        fringe.Add(temp);
                        //System.Console.WriteLine(newX + ", " + (newZ + 1) + " added to fringe up with cost of " + temp.TotalCost);
                    }
                }
                if (canMove(currentNode, "down"))
                {
                    Node temp = new Node(newX, newZ - 1, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - (newZ - 1));
                        temp.CostToGo = Math.Abs(endX - newX);
                        temp.TotalCost = temp.Cost + temp.CostToGo;
                        //visited.Enqueue(temp);
                        fringe.Add(temp);
                        //System.Console.WriteLine(newX + ", " + (newZ - 1) + " added to fringe down with cost of " + temp.TotalCost);
                    }
                }
                if (canMove(currentNode, "left"))
                {
                    //newX--;
                    Node temp = new Node(newX - 1, newZ, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - newZ);
                        temp.CostToGo = Math.Abs(endX - (newX - 1));
                        temp.TotalCost = temp.Cost + temp.CostToGo;
                        //visited.Enqueue(temp);
                        fringe.Add(temp);
                        //System.Console.WriteLine((newX - 1) + ", " + newZ + " added to fringe left with cost of " + temp.TotalCost);
                    }
                }
                if (canMove(currentNode, "right"))
                {
                    //newX++;
                    Node temp = new Node(newX + 1, newZ, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - newZ);
                        temp.CostToGo = Math.Abs(endX - (newX + 1));
                        temp.TotalCost = temp.Cost + temp.CostToGo;
                        //visited.Enqueue(temp);
                        fringe.Add(temp);
                        //System.Console.WriteLine((newX + 1) + ", " + newZ + " added to fringe right with cost of " + temp.TotalCost);
                    }
                }
                int size = fringe.Count;
                //System.Console.WriteLine(size);
                if (size == 0)
                {
                    blocked = true;
                    break;
                }
                fringe.Sort(myComparison);
                currentNode = (Node)fringe[0];
                //System.Console.WriteLine("Current Node set to " + currentNode.X + ", " + currentNode.Z);
                visited.Enqueue(currentNode);
                fringe.RemoveAt(0);
            }
            //System.Console.WriteLine("current node is " + currentNode.X + ", " + currentNode.Z);
            if (blocked)
            {
                //System.Console.WriteLine("NO PATH");
                path = null;
            }
            else
            {
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.Parent;
                }
                path.Reverse();
                //System.Console.WriteLine("The path that exists is " + path.Count + " nodes long");
            }
            return path;
        }

        public bool isPath()
        {
            ArrayList possiblePath = findPath();
            if (possiblePath == null)
            {
                return false;
            }
            return true;
        }

        public ArrayList getPath()
        {
            if (!isPath())
            {
                path = new ArrayList();
                Node temp = new Node(startX, startZ + 2, null);
                path.Add(temp);
                Node temp1 = new Node(endX, endZ + 2, null);
                temp1.Parent = temp;
                path.Add(temp1);
            }
            return path;
        }
    }
}

