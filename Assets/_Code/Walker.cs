using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Walker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CoinPurse coinPurse;
    public bool frozen = false;
    public VendorNode toNode;
    public VendorNode fromNode;

    public IntVariable supply;
    public Resource resource;
    public int resourceAmount;
    public int lastPaid;
    public int coins;
    
    private float investmentRemainder = 0;
    private float improvementRemainder = 0;
    private float profitRemainder = 0;

    public FloatVariable speed, investmentPercent, improvementPercent, profitPercent;

    public delegate void OnKill(Walker walker);
    public OnKill onKill;

    void Awake()
    {
        coinPurse = CoinPurse.instance;
    }

    void OnEnable()
    {
        frozen = false;
    }

    public void Update()
    {
        if(frozen) return;
        if(toNode == null) return;

        float step = speed.Value * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, toNode.transform.position, step);
        
        if(Vector3.Distance(transform.position, toNode.transform.position) < 0.001f)
        {
            Arrive();
        }
    }

    public void Arrive()
    {
        VendorNode temp = toNode;
        toNode = fromNode;
        fromNode = temp;

        Negotiate();
    }

    public void Negotiate()
    {
        Sell(fromNode);
        Buy(fromNode, toNode);
    }

    public void Buy(VendorNode buyFrom, VendorNode sellTo)
    {
        if(buyFrom == null) return;
        if(sellTo == null) return;

        Resource toBuy = GetResourceToBuy(buyFrom, sellTo);

        if(toBuy == null) return;

        int buyAmount = GetBuyAmount(toBuy);
        int buyCoins = GetBuyCoins(toBuy, buyAmount);

        resource = toBuy;
        resourceAmount = buyAmount;
        coins -=  buyCoins;
        lastPaid = buyCoins;
    }

    public Resource GetResourceToBuy(VendorNode buyFrom, VendorNode sellTo)
    {
        if(buyFrom == null) return null;
        if(sellTo == null) return null;

        int maxProfit = 0;
        Resource toReturn = null;

        foreach(Resource toBuy in buyFrom.sells)
        {
            int buyAmount = GetBuyAmount(toBuy);
            int buyPrice = GetBuyCoins(toBuy, buyAmount);
            int sellPrice = GetSaleCoins(sellTo, toBuy, buyAmount);
            int profit = sellPrice - buyPrice;

            if(profit >= maxProfit)
            {
                maxProfit = profit;
                toReturn = toBuy;
            }
        }

        return toReturn;
    }

    public int GetMaxCoinAmount()
    {
        int toCoins = GetMaxCoinAmount(toNode);
        int fromCoins = GetMaxCoinAmount(fromNode);

        int toReturn = (toCoins > fromCoins) ? toCoins : fromCoins;

        return toReturn;
    }

    public int GetMaxCoinAmount(VendorNode node)
    {
        if(node == null) return 0;
        
        int toReturn = 0;

        foreach(Resource resource in node.sells)
        {
            int buyCoins = GetBuyCoins(resource, supply.Value);
            if(buyCoins > toReturn) toReturn = buyCoins;
        }

        return toReturn;
    }

    public void Sell(VendorNode node)
    {
        if(node == null) return;
        if(resource == null) return;

        int sellCoins = GetSaleCoins(node, resource, resourceAmount);
        
        int profit = sellCoins - lastPaid;

        if(sellCoins >= lastPaid) coins += lastPaid;
        else coins += sellCoins;

        resourceAmount = 0;
        
        HandleProfit(profit);
    }

    public int GetBuyAmount(Resource resource)
    {
        if(resource == null) return 0;
        if(resource.moneyValue == 0) return 0;

        return Mathf.Clamp(coins / resource.moneyValue, 0, supply.Value);
    }

    public int GetBuyCoins(Resource resource, int amount)
    {
        if(resource == null) return 0;

        return resource.moneyValue * amount;
    }

    public int GetSaleCoins(VendorNode node, Resource resource, int amount)
    {
        if(node == null) return 0;
        if(resource == null) return 0;

        float buyFactor = node.GetBuyFactor(resource);
        int buyCoins = Mathf.FloorToInt(buyFactor * resource.moneyValue * amount);
        
        return buyCoins;
    }
    
    public void HandleProfit(int profit)
    {
        if(profit <= 0) return;

        investmentRemainder += investmentPercent.Value * profit;
        improvementRemainder += improvementPercent.Value * profit;
        profitRemainder +=  profitPercent.Value * profit;

        int investment = Mathf.FloorToInt(investmentRemainder);
        int improvement = Mathf.FloorToInt(improvementRemainder);
        int profitReturn = Mathf.FloorToInt(profitRemainder);

        investmentRemainder -= investment;
        improvementRemainder -= improvement;
        profitRemainder -= profitReturn;

        HandleProfitReturn(profitReturn);
        HandleImprovement(improvement);
        HandleInvestment(investment);
    }

    public void HandleProfitReturn(int profitReturn)
    {
        if(profitReturn > 0)
            coinPurse.Adjust(profitReturn);

    }

    public void HandleImprovement(int improvement)
    {
        if(coins < GetMaxCoinAmount())
            coins += improvement;
    }

    public void HandleInvestment(int investment)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onKill != null) onKill(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        frozen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        frozen = false;
    }
}
