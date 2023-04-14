using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationElement : MonoBehaviour
{
    private void OnEnable()
    {
        RotatorBrain.OnSendRotationObject?.Invoke(this.transform);
    }
}
