using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Garden : InteractiveZone
{
    [SerializeField] private Transform _startSpawnPosition; 
    [SerializeField] private int _maxItemsCount = 3;
    [SerializeField] private int _spawnDelayInMs = 500;
    [SerializeField] private List<Transform> _targetItemsPositions;
    [SerializeField] private List<BaseItem> _targetItems;
    [SerializeField] private BaseItem.ItemType _type;


    private void Start()
    {
        OnTriggeredPlayer += CalmObjects;
        OnStateUpdate += CalmObjects;
        BuyerController.OnAddNewItemtype?.Invoke(_type);
        _targetItems = new List<BaseItem>();
        SpawnItem();
    }

    private void CalmObjects()
    {
        if (_targetUnits.Count == 0) return;
        _zoneIsProcess = true;
        foreach(Unit unit in _targetUnits)
        {
            Debug.Log("Calm objects");
            if (_targetItems.Count != 0)
            {
                for (int i = 0; i < _targetItems.Count; i++)
                {
                    BaseItem item = _targetItems[i];
                    bool isfull = unit.Inventory.PutInInventory(item, true);
                    if(!isfull)
                    _targetItems.Remove(item);
                }
            }
        }
        _zoneIsProcess = false;
    }

    private async void SpawnItem()
    {
        while(Application.isPlaying)
        {
            while (Application.isPlaying && _targetItems.Count == _maxItemsCount)
            {
                await Task.Yield();
            }
            Debug.Log("Spawn Item in Garden");
            
            if(_targetItems.Count <= _maxItemsCount - 1)
            {
                BaseItem TempItem = ItemsPool.OnGetBaseItem?.Invoke(_type);
                await Task.Delay(TempItem._delayInMs);
                TempItem.gameObject.SetActive(true);
                TempItem.transform.position = _startSpawnPosition.position;
                _targetItems.Add(TempItem);
                AnimateItem(TempItem.transform);
            }
        }
    }

    private void AnimateItem(Transform item)
    {
        for (int i = 0; i < _targetItemsPositions.Count; i++)
        {
            if(_targetItemsPositions[i].childCount == 0)
            {
                Debug.Log("Animated");
                item.SetParent(_targetItemsPositions[i]);
                item.DOMove(_targetItemsPositions[i].position, 0.5f).OnComplete(() => { if (_isStayOnPlane) CalmObjects(); });
                return;
            }
        }
    }
}
