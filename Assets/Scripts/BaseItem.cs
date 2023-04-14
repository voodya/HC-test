using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    [SerializeField] public int _price;
    [SerializeField] public int _delayInMs;
    [SerializeField] public ItemType _type;

    public enum ItemType 
    {
        Apple,
        Tomato,
    }
}
