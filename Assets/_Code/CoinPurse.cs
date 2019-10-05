using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPurse : MonoBehaviour
{
    public static CoinPurse instance;
    public int coins;

    public delegate void OnAdjust(int amount);
    public OnAdjust onAdjust;

    public delegate void OnChanged();
    public OnChanged onChanged;

    void OnEnable()
    {
        if(instance == null) instance = this;
        else
        {
            Destroy(this);
        }
    }

    public bool CheckChange(int amount)
    {
        return coins + amount > 0;
    }

    public void Adjust(int amount)
    {
        if(CheckChange(amount))
        {
            coins += amount;

            if(onAdjust != null) onAdjust(amount);
            if(onChanged != null) onChanged();
        }
    }
}
