using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {

    private SpriteRenderer spriteRenderer;

    //reference to range check on tower
    private SpriteRenderer rangedSpriteRenderer;

    public bool IsVisible { get;private set; }

    // Use this for initialization
    void Start () {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangedSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();  //gets first child of the hover icon which will be the range    
	}
	
	// Update is called once per frame
	void Update () {
        FollowMouse();
	}

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);   //follow mouse
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);    //sorts out Z position so 2d sprite isn't lost behind other objects
        }
    }

    public void Activate(Sprite sprite)     //allows you to attach the sprite of a tower to the pointer (drag and drop...kinda)
    {
        this.spriteRenderer.sprite = sprite;
        this.spriteRenderer.enabled = true;
        rangedSpriteRenderer.enabled = true;
        IsVisible = true;
    }

    public void Deavtivate()
    {
        spriteRenderer.enabled = false;
        GameManager.Instance.ClickedBtn = null;         //turn off so tower isn't placed
        rangedSpriteRenderer.enabled = false;
        IsVisible = false;
    }
}
