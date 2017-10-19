using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image content;

    public float MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        HandleBar();
	}

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)   //only update if different
        {
            content.fillAmount = fillAmount;
        }
    }

    public void Reset()
    {
        Value = MaxValue;
        content.fillAmount = 1;
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)    // actual value, min value, max value, min/max output value
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //e.g (80-0) * (1 - 0) / (100 - 0) + 0
        //     80 * 1 / 100 == 0.8

    }
}
