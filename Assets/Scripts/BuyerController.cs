using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class BuyerController : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _cashPosition;
    [SerializeField] private List<PositionsShelf> _positions;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private Buyer _prefab;
    [SerializeField] private List<Transform> _queuePositions;
    [SerializeField] private List<bool> _queuePositionsState;
    [SerializeField] private int _buyerCount;
    [SerializeField] private int _buyerSpawnDelay = 10000;

    [SerializeField] private BuyerPattern _pattern;
    [SerializeField] private List<BaseItem.ItemType> _allowedItemsTypes;

    public Action<Buyer> OnItemsGetted;
    public static Action<BaseItem.ItemType> OnAddNewItemtype;
    public Action OnAddedNewItemType;
    public Action OnQueueMoved;

    private int _queueCount = 0;
    private List<(Buyer, int)> _buyersInQueue;
    private List<Buyer> _freeBuyers;

    private void Awake()
    {
        OnItemsGetted += QueueWait;
        OnAddNewItemtype += AddNewItemType;
        _buyersInQueue = new List<(Buyer, int)>();
    }

    private void AddNewItemType(BaseItem.ItemType obj)
    {
        Debug.Log($"Add type {obj}");
        _allowedItemsTypes.Add(obj);
    }

    private async void Start()
    {

        for (int i = 0; i < _buyerCount; i++)
        {
            await Task.Delay(_buyerSpawnDelay);
            SpawnBuyer();
        }
    }

    public void OutMarket(Buyer obj)
    {
        if(_queueCount != 0)
        {
            _queueCount--;
            obj.GoTo(_endPosition.position);
            _buyersInQueue.Remove((obj, 0));
            MoveQueue();
        }
    }

    public void RebuildBuyer(Buyer buyer)
    {
        buyer.transform.position = _startPosition.position;

        List<ItemsPattern> items = GetAllowedPatterns();

        buyer.UpdateBuyer(items, this);

        //buyer.UpdateBuyer(3, "Tomato", this); //add pattern
        buyer.GoTo(_positions.Find(x => x._type == items[0]._type)._pos.position);
    }

    private void SpawnBuyer()
    {

        List<ItemsPattern> items = GetAllowedPatterns();

        var f = Instantiate(_prefab);
        f.transform.position = _startPosition.position;
        f.UpdateBuyer(items, this); //add pattern


        f.GoTo(_positions.Find(x => x._type == items[0]._type)._pos.position);
    }

    private void QueueWait(Buyer npc)
    { 
        for (int i = 0; i < npc._itemsPatterns.Count; i++)
        {
            if (npc._itemsPatterns[i]._isDone)
            {
                Debug.Log($"Pattern {npc._itemsPatterns[i]._type} is Done");
                //if (i == npc._itemsPatterns.Count - 1) break; 
                //else
                continue;
            }
            else
            {
                npc.GoTo(_positions.Find(x => x._type == npc._itemsPatterns[i]._type)._pos.position);
                return;
            }  
        }
        npc.GoTo(_queuePositions[_queueCount].position);
        _buyersInQueue.Add((npc, _queueCount));
        _queueCount++;
    }

    private void MoveQueue()
    {
        for (int i = 0; i < _buyersInQueue.Count; i++)
        {
            (Buyer, int) temp = new(_buyersInQueue[i].Item1, _buyersInQueue[i].Item2 == 0 ? _buyersInQueue[i].Item2: _buyersInQueue[i].Item2 -1);
            _buyersInQueue[i] = temp;
            temp.Item1.GoTo(_queuePositions[temp.Item2].position);
        }
    }
     

    private List<ItemsPattern> GetAllowedPatterns()
    {
        
        List<ItemsPattern> tamp = _pattern._partsPattern.FindAll(x => _allowedItemsTypes.Contains(x._type));
        Debug.LogWarning(tamp.Count);
        return tamp;
    }
}

[System.Serializable]
public class ItemsPattern
{
    public int _objectsCount;
    public BaseItem.ItemType _type;
    public bool _isDone = false;
}

[System.Serializable]
public class BuyerPattern
{
    public List<ItemsPattern> _partsPattern;
}

[System.Serializable]
public class PositionsShelf
{
    public Transform _pos;
    public BaseItem.ItemType _type;

}


