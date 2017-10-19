using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickMenu : MonoBehaviour {

    private Canvas myCanvas;

    // Use this for initialization
    void Start()
    {
        this.myCanvas = GetComponentInParent<Canvas>(); //gets the main canvas
    }

    // Update is called once per frame
    void Update()
    {
        SpawnMenu();
    }

    private void SpawnMenu()
    {
            if (Input.GetMouseButtonDown(0))
            {
            //The below 3 lines allow the menu to appear where the mouse is on click
            // more info found http://answers.unity3d.com/questions/849117/46-ui-image-follow-mouse-position.html
            Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
                transform.position = myCanvas.transform.TransformPoint(pos);
            }

    }
}
