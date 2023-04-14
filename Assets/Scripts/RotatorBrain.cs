using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RotatorBrain : MonoBehaviour
{
    public static Action<Transform> OnSendRotationObject;
    [SerializeField] private Transform _targetPos; 

    private List<Transform> _objects;

    private void Awake()
    {
        OnSendRotationObject += CollectObjs;
        _objects = new List<Transform>();
    }

    private void CollectObjs(Transform obj)
    {
        _objects.Add(obj);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].rotation = Quaternion.LookRotation(_targetPos.position);
        }
    }
}
