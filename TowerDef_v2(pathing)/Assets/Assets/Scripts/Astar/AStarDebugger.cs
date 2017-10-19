using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour {
    //debugging script DELETE LATER

    [SerializeField]
    private TileScript start, goal;
    [SerializeField]
    private Sprite blankTile;
    [SerializeField]
    private Sprite stoneTile;
    [SerializeField]
    private GameObject arrowPrefab;


    [SerializeField]
    private GameObject debugTilePrefab;

	// Use this for initialization
	void Start () {
		
	}

    //// Update is called once per frame
    //void Update()
    //{
    //    ClickTile();

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        AStar.GetPath(start.GridPosition, goal.GridPosition);
    //    }
    //}

    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);    // makes a raycast from mouse position to the start

            if(hit.collider != null )
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();   //equal to tile hit by mouse
                if (tmp != null)    //if actually hit a tile not a tower etc
                {
                    if (start == null)
                    {
                        start = tmp;    //make selected tile start point
                        CreateDebugTIle(start.WorldPosition, new Color32(255, 135, 0, 255));
                    }
                    else if (goal == null)
                    {
                        goal = tmp;     //second click = goal
                        CreateDebugTIle(goal.WorldPosition, new Color32(255, 0, 0, 255));
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path)
    {
        foreach (Node node in openList)
        {
            if(node.TileRef != start && node.TileRef != goal)
            {
                CreateDebugTIle(node.TileRef.WorldPosition, Color.cyan, node);
            }
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in closedList)
        {
            
            if (node.TileRef != start && node.TileRef != goal && path.Contains(node))   //only make a blue tile if final path doesn't contain it
            {
                CreateDebugTIle(node.TileRef.WorldPosition, Color.blue, node);
            }
            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (Node node in path)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
                CreateDebugTIle(node.TileRef.WorldPosition, Color.green, node);
            }
        }
    }



    private void PointToParent(Node node, Vector2 position)     //point the arrow to the parent
    {
        if (node.Parent != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;  //puts arrow on top

            if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y == node.Parent.GridPosition.Y)  //point right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y)  //point top right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            else if (node.GridPosition.X == node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y)  //point top up
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y > node.Parent.GridPosition.Y)  //point top left
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y == node.Parent.GridPosition.Y)  //point right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (node.GridPosition.X < node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y)  //point bottom right
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -45);
            }
            else if (node.GridPosition.X == node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y)  //point down
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else if (node.GridPosition.X > node.Parent.GridPosition.X && node.GridPosition.Y < node.Parent.GridPosition.Y)  //point bottom left
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -135);
            }
        }

    }

    private void CreateDebugTIle(Vector3 worldPos, Color32 color, Node node = null) //set to null to make it optional 
    {
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos,Quaternion.identity);

        if (node != null)
        {
            DebugTile tmp = debugTile.GetComponent<DebugTile>();    //only have to get debug tile component once to save resources

            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;
        }

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
