using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinIndicator : MonoBehaviour
{
    private CoinPurse coinPurse;
    public TextMeshProUGUI text;

    void Start()
    {
        coinPurse = CoinPurse.instance;
        coinPurse.onChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        text.SetText("Coins: " + coinPurse.coins.ToString());
    }
}
