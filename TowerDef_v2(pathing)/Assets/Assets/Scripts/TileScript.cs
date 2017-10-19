using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{

    public Point GridPosition { get; private set; }

    public bool IsEmpty { get;  set; }   //checks if the tile is empty or not

    private Tower myTower;

    private Color32 fullColor = new Color32(255, 188, 188, 255);    //tile highlight color when full

    private Color32 emptyColor = new Color32(96,255,90,255);    //tile highlight color when empty

    private SpriteRenderer spriteRenderer;

    public bool WalkAble;

    public bool Debugging { get; set; }
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
        }
    }


    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        WalkAble = true;
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);

    }

    private void OnMouseOver()
    {
       // will only execute when pointer is not over a gameobject
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)  //clicked button not null when carrying tower
        {
            if (IsEmpty && !Debugging)
            {
                ColorTile(emptyColor);
            }
            if (!IsEmpty && !Debugging)
            {
                ColorTile(fullColor);
            }
            else if (Input.GetMouseButtonDown(0))   //will only try to place tower if empty
            {
                PlaceTower();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0)) //if there is a tower on the tile then
        {
            if (myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (IsEmpty && !Debugging)
        //    {
        //        ColorTile(emptyColor);
        //    }
        //    if (!IsEmpty && !Debugging)
        //    {
        //        ColorTile(fullColor);
        //    }
        //}
    }

    private void OnMouseExit()
    {
        if (!Debugging)
        {
            ColorTile(Color.white);
        }

    }

    private void PlaceTower()
    {
        WalkAble = false;
        if(AStar.GetPath(LevelManager.Instance.PortalSpawn,LevelManager.Instance.CoinSpawn) == null)
        {
            WalkAble = true;
            return; //no path ie tower block
        }
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);     //quanternion so it does not rotate

        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y; //stops the towers overlapping 
        tower.transform.SetParent(transform);       //makes the tower a child of the tiles its on
        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();   //get the tower script

        IsEmpty = false;

        ColorTile(Color.white);

        myTower.Price = GameManager.Instance.ClickedBtn.Price;     

        GameManager.Instance.BuyTower();

        WalkAble = false; // tower on point so no longer "walkable"
    }

    private void ColorTile(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
