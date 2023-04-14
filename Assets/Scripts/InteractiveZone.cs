using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(BoxCollider))]
public class InteractiveZone : MonoBehaviour
{
    [SerializeField] private bool _isAnimated;
    [SerializeField] private int _delay = 300;
    [SerializeField] private Transform _animObj;

    protected Action OnTriggeredPlayer;
    protected Action OnTriggeredBuyer;
    protected Action OnStateUpdate;
    protected bool _isStayOnPlane;
    protected List<Unit> _targetUnits = new List<Unit>();

    protected bool _zoneIsProcess = false;

    private void Start()
    {
        _targetUnits = new List<Unit>();
    }

    private async void OnTriggerEnter(Collider other)
    {
        _isStayOnPlane = true;
        if (other.tag == "Player")
        {
            Debug.Log("Player enter");
            Inventory inventory = null;
            Debug.Log(other.TryGetComponent(out inventory));
            _targetUnits.Add(new Unit(inventory, true, other.name));
            if (_isAnimated) _animObj.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.5f).SetEase(Ease.OutBounce);
            await Task.Delay(_delay);
            OnTriggeredPlayer?.Invoke();
            
        }
        else
        {
            Debug.Log("Buyer enter");
            _targetUnits.Add(new Unit(other.GetComponent<Inventory>(), false, other.name));
            OnTriggeredBuyer?.Invoke();
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        _isStayOnPlane = false;
        if (other.tag == "Player" && _isAnimated) _animObj.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBounce);
        else Debug.Log("Buyer exit");
        _targetUnits.Remove(_targetUnits.Find(x => x.Name == other.name));
    }

    private void OnTriggerStay(Collider other)
    {
        if (_zoneIsProcess) return;
        OnStateUpdate?.Invoke();
    }
}

public class Unit
{
    public Inventory Inventory;
    public bool IsPlayer;
    public string Name;

    public Unit(Inventory inventory, bool isPlayer, string name)
    {
        Inventory = inventory;
        IsPlayer = isPlayer;
        Name = name;
    }
}

