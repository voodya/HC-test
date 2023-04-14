using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _money;
    [SerializeField] private TextMeshProUGUI _walletVis;



    public void AddMoney(int count)
    {
        _money += count;
        UpdateVisual();
    }

    public bool WasteMoney(int count)
    {
        if(_money-count < 0) return false;
        _money -= count;
        UpdateVisual();
        return true;
    }

    public bool CheckMoney(int cost)
    {
        return _money > cost;
    }

    private void UpdateVisual()
    {
        _walletVis.text = _money.ToString();
    }
}
