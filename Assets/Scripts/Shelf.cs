using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Shelf : InteractiveZone
{
    [SerializeField] private List<Transform> _shelfObjPositions;
    [SerializeField] private List<BaseItem> _itemsInShelf;
    [SerializeField] private BaseItem.ItemType _type;

    private bool _isProcess = false;
    
    private void Awake()
    {
        OnTriggeredPlayer += CollectToShelf;
        OnTriggeredBuyer += RemoveToShelf;
        OnStateUpdate += RemoveToShelf;
    }

    private async void RemoveToShelf()
    {
        if (_zoneIsProcess) return;
        _zoneIsProcess = true;
        foreach (Unit unit in _targetUnits)
        {
            if (unit.IsPlayer) continue;
            if (_itemsInShelf.Count != 0)
            {
                for (int i = 0; i < _itemsInShelf.Count; i++)
                {
                    bool isDone = unit.Inventory.PutInInventory(_itemsInShelf[i], false);
                    if(!isDone)
                    _itemsInShelf.Remove(_itemsInShelf[i]);
                }
            }
        }
        _zoneIsProcess = false;
    }

    private void CollectToShelf()
    {
        if (_targetUnits.Count == 0) return;
        _zoneIsProcess = true;
        for (int i = 0; i< _targetUnits.Count; i++)
        {
            Debug.Log(_targetUnits[i].IsPlayer);
            if (!_targetUnits[i].IsPlayer) continue;
            int gettedItems = 0;
            foreach (BaseItem item in _targetUnits[i].Inventory._items)
            {
                if(item._type != _type) continue;
                if (_itemsInShelf.Count >= _shelfObjPositions.Count) break;
                _itemsInShelf.Add(item);
                
                MoveToShelf(item);
                gettedItems++;
            }
            _targetUnits[i].Inventory.TakeFromInventory(gettedItems);
            
        }
        _zoneIsProcess = false;
    }

    private void MoveToShelf(BaseItem item)
    {
        for (int i = 0; i < _shelfObjPositions.Count; i++)
        {
            if (_shelfObjPositions[i].childCount == 0)
            {
                Debug.Log("Animated");
                item.transform.SetParent(_shelfObjPositions[i]);
                item.transform.DOMove(_shelfObjPositions[i].position, 0.5f);

                return;
            }
        }
    }
}
