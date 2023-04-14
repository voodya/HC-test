using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBuyerCicle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Buyer>().ForseUpdate();
    }
}
