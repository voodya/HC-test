using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : InteractiveZone
{
    [SerializeField] private List<Transform> _moneyPos;
    [SerializeField] private List<Transform> _itemPos;
    [SerializeField] private Wallet _wallet;
    
    private void Awake()
    {
        OnTriggeredBuyer += BuyeItems;
        OnTriggeredPlayer += SellItems;
    }

    private void SellItems()
    {
        int price = 0;
        for (int i = 0; i < _targetUnits.Count; i++)
        {
            if (_targetUnits[i].IsPlayer) continue;
            foreach (BaseItem t in _targetUnits[i].Inventory._items)
            {
                price += t._price;
                ItemsPool.OnReturnItem?.Invoke(t);
            }
            _targetUnits[i].Inventory.TakeFromInventory(_targetUnits[i].Inventory._items.Count);
            _wallet.AddMoney(price);
        }
    }

    private void BuyeItems()
    {
        //throw new NotImplementedException();
    }
}
