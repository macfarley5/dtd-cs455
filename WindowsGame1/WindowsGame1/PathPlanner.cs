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
        Map myMap;
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

        public PathPlanner(int inWidth, int inDepth, int inStartX, int inStartZ, int inEndX, int inEndZ)
        {
            path = new ArrayList();
            visited = new Queue();
            startX = inStartX;
            startZ = inStartZ;
            endX = inEndX;
            endZ = inEndZ;
            width = inWidth;
            depth = inDepth;
        }

        private bool canMove(Node inNode, string dir)
        {
            bool moveAllowed = true;
            if (dir == "up")
            {
                if (inNode.Z + 1 >= depth)
                {
                    moveAllowed = false;
                }
                if (myMap.isOccupied(inNode.X, inNode.Z - 1))
                {
                    moveAllowed = false;
                }
            }
            else if (dir == "down")
            {
                if (inNode.Z - 1 <= depth)
                {
                    moveAllowed = false;
                }
                if (myMap.isOccupied(inNode.X, inNode.Z - 1))
                {
                    moveAllowed = false;
                }
            }
            else if (dir == "left")
            {
                if (inNode.x - 1 <= width)
                {
                    moveAllowed = false;
                }
                if (myMap.isOccupied(inNode.X - 1, inNode.Z))
                {
                    moveAllowed = false;
                }
            }
            else if (dir == "right")
            {
                if (inNode.X + 1 >= width)
                {
                    moveAllowed = false;
                }
                if (myMap.isOccupied(inNode.X + 1, inNode.Z))
                {
                    moveAllowed = false;
                }
            }
            else
            {
                moveAllowed = false;
            }
            return moveAllowed;
        }

        public bool beenVisited(Node inNode)
        {
            return visited.Contains(inNode);
        }

        private ArrayList findPath()
        {
            IComparer myComparison = new nodeComparer();
            ArrayList fringe = new ArrayList();
            Node currentNode = new Node(startX, startZ, null);
            Node goalNode = new Node(endX, endZ, null);
            //start at the beginning and go to the end
            bool blocked = false;
            bool left = false;
            bool right = false;
            bool up = false;
            bool down = false;
            while (currentNode != goalNode)
            {
                left = true;
                right = true;
                up = true;
                down = true;
                int newX = currentNode.X;
                int newZ = currentNode.Z;
                if (canMove(currentNode, "up"))
                {
                    newZ++;
                    Node temp = new Node(newX, newZ, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - newZ);
                        temp.CostToGo = endX - newX;
                        visited.Enqueue(temp);
                        fringe.Add(temp);
                    }
                }
                else if (canMove(currentNode, "down"))
                {
                    newZ--;
                    Node temp = new Node(newX, newZ, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - newZ);
                        temp.CostToGo = endX - newX;
                        visited.Enqueue(temp);
                        fringe.Add(temp);
                    }
                }
                else if (canMove(currentNode, "left"))
                {
                    newX--;
                    Node temp = new Node(newX, newZ, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - newZ);
                        temp.CostToGo = endX - newX;
                        visited.Enqueue(temp);
                        fringe.Add(temp);
                    }
                }
                else if (canMove(currentNode, "right"))
                {
                    newX++;
                    Node temp = new Node(newX, newZ, currentNode);
                    if (!beenVisited(temp))
                    {
                        temp.Cost = Math.Abs(endZ - newZ);
                        temp.CostToGo = endX - newX;
                        visited.Enqueue(temp);
                        fringe.Add(temp);
                    }
                }
                int size = fringe.Count;
                if (size == 0)
                {
                    blocked = true;
                    break;
                }
                fringe.Sort(myComparison);
                currentNode = (Node)fringe[0];
                fringe.RemoveAt(0);
            }
            if (blocked)
            {
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
            }
            return path;
        }

        bool isPath()
        {
            ArrayList possiblePath = findPath();
            if (possiblePath == null)
            {
                return false;
            }
            return true;
        }

        ArrayList getPath()
        {
            return path;
        }
    }
}

