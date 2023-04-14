using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] public int _inventorySize;
    [SerializeField] public List<ItemsPattern> _itemsPattern;
    [SerializeField] public List<BaseItem> _items;
    [SerializeField] private List<Transform> _positionsInventory;
    [SerializeField] private List<bool> _positionsState;


    public Action<List<BaseItem>> OnTargetStateChanget;
    public Action OnSellItems;

    private void Awake()
    {
        _positionsState = new List<bool>();
        foreach (var item in _positionsInventory)
        {
            _positionsState.Add(false);
        }
    }

    public bool PutInInventory(BaseItem item, bool isPlayer)
    {
        if(isPlayer)
        {
            if (_items.Count < _inventorySize)
            {
                _items.Add(item);
                MoveToInventory(item.transform);
                OnTargetStateChanget?.Invoke(_items);
                return false;
            }
            else
                return true;
        }
        else
        {
            foreach (var pattern in _itemsPattern)
            {
                if (item._type != pattern._type) continue;
                if (_items.FindAll(x => x._type == pattern._type).Count < pattern._objectsCount)
                {
                    _items.Add(item);
                    MoveToInventory(item.transform);
                    OnTargetStateChanget?.Invoke(_items);
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        
    }

    public void TakeFromInventory(int count = 0)
    {
        List<BaseItem> newList = new List<BaseItem>();
        for (int i = _items.Count; i > 0; i--)
        {
            if(i > count)
            {
                newList.Add(_items[i]);
            }
            else
            {
                _positionsState[i - 1] = false;
            }
        }
        _items = newList;
        if (_items.Count == 0) OnSellItems?.Invoke();
    }

    private void MoveToInventory(Transform item)
    {
        for (int i = 0; i < _positionsInventory.Count; i++)
        {
            if (!_positionsState[i])
            {
                Debug.Log("Animated");
                item.DOLocalMove(_positionsInventory[i].localPosition, 0.5f);
                item.SetParent(_positionsInventory[i]);
                _positionsState[i] = true;
                return;
            }
        }
    }
}
