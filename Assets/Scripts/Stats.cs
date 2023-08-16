using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    [SerializeField]
    private BarScript bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

    public float CurrentValue
    {
        get
        {
            return currentVal;
        }

        set
        {
            // this stops the health from counting over the max health 
            this.currentVal = Mathf.Clamp(value, 0, MaxVal);

            bar.Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            bar.MaxValue = maxVal;
            this.maxVal = value;
            
        }
    }
    
    public void Initialize()
    {
        //Updates the bar
        this.MaxVal = maxVal;
        this.CurrentValue = currentVal;
    }
}
