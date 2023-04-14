using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuilderPlace : InteractiveZone
{
    [SerializeField] private int _cost;
    [SerializeField] private GameObject _build;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private float _yOffset;

    private void Awake()
    {
        OnTriggeredPlayer += BuySomthingBuild;
        _costText.text = _cost.ToString();
    }

    private void BuySomthingBuild()
    {
        if (!_wallet.CheckMoney(_cost)) return;
        _wallet.WasteMoney(_cost);
        Instantiate(_build).transform.position = new(transform.position.x, transform.position.y + _yOffset, transform.position.z);
        Destroy(gameObject);
    }
}
