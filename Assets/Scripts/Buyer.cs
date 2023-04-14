using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

[RequireComponent(typeof(Inventory), typeof(NavMeshAgent))]
public class Buyer : MonoBehaviour
{
    private int _itemCount;
    private BaseItem.ItemType _itemType;
    private NavMeshAgent _agent;
    private Inventory _inventory;
    private Vector3 _position;
    private BuyerController _controller;
    public List<ItemsPattern> _itemsPatterns;

    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _inventory = GetComponent<Inventory>();
        _inventory.OnTargetStateChanget += CheckInventory;
        _inventory.OnSellItems += Selled;
    }

    private void Selled()
    {
        _controller.OutMarket(this);
    }

    private void CheckInventory(List<BaseItem> obj)
    {
        foreach (ItemsPattern pattern in _itemsPatterns)
        {
            if ( pattern._isDone) continue;

            if (obj.FindAll(x => x._type == pattern._type).Count == pattern._objectsCount)
            {
                Debug.LogError($"Pattern {pattern._type} is Done");
                pattern._isDone = true;
                _controller.OnItemsGetted?.Invoke(this);
            }
        }
    }

    public void ForseUpdate()
    {
        _controller.RebuildBuyer(this);
    }

    public void UpdateBuyer(int count, BaseItem.ItemType type, BuyerController controller)
    {
        _itemCount = count;
        _itemType = type;
        _controller = controller;
        _inventory._inventorySize = count;
    }

    public void UpdateBuyer(List<ItemsPattern> list, BuyerController controller)
    {

        _text.text = "";

        _itemsPatterns = list;
        int f = 0;
        foreach(ItemsPattern pat in list)
        {
            _text.text += $"{pat._type}/{pat._objectsCount}\n";
            f += pat._objectsCount;
            pat._isDone = false;
        }
        _inventory._inventorySize = f;
        _inventory._itemsPattern = list;
        _controller = controller;
    }

    public void GoTo(Vector3 pos, bool kill = false)
    {
        if(_agent.SetDestination(pos) && kill)
            Destroy(gameObject);
    }

}
