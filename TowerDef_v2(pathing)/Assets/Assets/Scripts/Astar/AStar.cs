using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar  {

    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        //run through all tiles in game
        foreach  (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));       //add nodes to dictionary
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {
        if (nodes == null)  //if nodes have not been made create them
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();       //open list

        HashSet<Node> closedList = new HashSet<Node>();       //closed list

        Stack<Node> finalPath = new Stack<Node>();       //backtrack for creeps so you can pop from start to end

        Node currentNode = nodes[start];    //insitalise starting path

        //http://www.policyalmanac.org/games/aStarTutorial.htm
        openList.Add(currentNode);  //step 1: add start point to open list
        while (openList.Count > 0)
        {
            //check neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y); //get neighbour tiles positions from (-1, -1) to (1, 1) 

                    if (LevelManager.Instance.InBounds(neighbourPos)
                        && LevelManager.Instance.Tiles[neighbourPos].WalkAble && neighbourPos != currentNode.GridPosition)   //neighbour different to current pos, and walkable
                    {
                        int gCost = 0;

                        if (Math.Abs(x - y) == 1)   //if horizontal or vertical Absoulute value means -1 = 1
                        {
                            gCost = 10;
                        }
                        else //else diagonal
                        {
                            if(!ConnectedDiagonally(currentNode,nodes[neighbourPos]))
                            {                          
                                continue; //go to next execution in loop
                            }
                            gCost = 14;
                        }

                        Node neighbour = nodes[neighbourPos];   //get the node of that tile

                        if (openList.Contains(neighbour))   //step 6: if an adjacent square is already on openlist check to see if path is better to go directly to this square
                        {
                            if (currentNode.G + gCost < neighbour.G)    //check if current node is actually a better parent based all new gScore value
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }
                        else if (!closedList.Contains(neighbour))   //if not already checked then add to openlist
                        {
                            openList.Add(neighbour);    //add new neighbour tile to openlist         step 2: add all other tiles to open list
                            neighbour.CalcValues(currentNode, nodes[goal], gCost);  //calc values for parent... might not be optimal to do this here
                        }
                    }
                }
            }

            // moves current node from open to closed list
            openList.Remove(currentNode);   //step 3: remove starting tile from open list
            closedList.Add(currentNode);    //step 4: add it to the closed list

            if (openList.Count > 0)     //step 5: check all adjacent squares
            {
                currentNode = openList.OrderBy(n => n.F).First();   //sorts list by F value and selects the first on the list
            }

            if(currentNode == nodes[goal])  //once goal is in list then go back and add to final path stack
            {
                while (currentNode.GridPosition != start)   //stop when you reach the start
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                // GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);  //getting called too many times? //ONLY FOR DEBUGGING REMOVE LATER need to sort this mess XD
                // break;  //we found the goal
                return finalPath;
            }
            

        }
       //  GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
       //   return finalPath;
        return null;    //no path found

    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;        //get direction of movement

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);    // first tile to check

        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y); //second tile to check

        if(LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].WalkAble)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].WalkAble)
        {
            return false;
        }

        return true;
    }
}
