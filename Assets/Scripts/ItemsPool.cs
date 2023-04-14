using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemsPool : MonoBehaviour
{
    [SerializeField] private List<BaseItem> _baseItems;
    [SerializeField] private Transform _poolTransform;
    [SerializeField] private List<ItemsPrefabs> _prefabs;
    [SerializeField] private BaseItem _baseItemDefault;

    public static Action<BaseItem> OnReturnItem;
    public delegate BaseItem GetBaseItem(BaseItem.ItemType type);
    public static GetBaseItem OnGetBaseItem;

    private void Awake()
    {
        OnReturnItem += ReturnItem;
        OnGetBaseItem += GetItem;
    }

    public BaseItem GetItem(BaseItem.ItemType _type)
    {
        if (_baseItems.Count != 0 && _baseItems.FindAll(x => !x.gameObject.activeSelf && x._type == _type).Count != 0)
            return _baseItems.Find(x => !x.gameObject.activeSelf && x._type == _type);
        else
        {
            BaseItem toInstance = _baseItemDefault;
            foreach (var a in _prefabs)
            {
                if (a._type == _type)
                    toInstance = a._item;
            }
            
            BaseItem TempItem = Instantiate(toInstance, _poolTransform);
            _baseItems.Add(TempItem);
            return TempItem;
        }
    }

    public void ReturnItem(BaseItem item)
    {
        item.transform.SetParent(_poolTransform);
        item.transform.position = _poolTransform.position;
        item.gameObject.SetActive(false);
    }
}
[System.Serializable]
public class ItemsPrefabs
{
    public BaseItem _item;
    public BaseItem.ItemType _type;
}

