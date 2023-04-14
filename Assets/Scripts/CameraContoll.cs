using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoll : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;



    public void FixedUpdate()
    {
        transform.position = new(_player.position.x + _offset.x, _player.position.y + _offset.y, _player.position.z + _offset.z);
    }


    [ContextMenu("Test")]
    public void CalcOffset()
    {
        _offset =  transform.position - _player.position;
    }
}
