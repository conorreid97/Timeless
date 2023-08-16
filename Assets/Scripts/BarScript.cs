using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

    private float fillAmount;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Text valueText;

    public float MaxValue { get; set; }

    // this updates the fillAmount
    public float Value
    {
        set
        {
            string[] tmp = valueText.text.Split(':');
            valueText.text = tmp[0] + ": " + value;
            fillAmount = Map(value, 0, MaxValue, 0, 1);
        }
    }

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        // if the fillAmount changes from the current then update it
        if (fillAmount != content.fillAmount)
        {
            HandleBar();
        }
    }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpSpeed);
        }
    }

    // the parameters mean - health value of player, min & max value of players health
    // then the min & max for the fill amount slider in inspector for "content"
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        // this takes the players health and translates it to a number between 0 - 1 for the fill amount slider
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
