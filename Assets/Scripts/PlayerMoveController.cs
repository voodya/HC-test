using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private DynamicJoystick _joystick;
    [SerializeField] private Inventory _inventory;
    private Rigidbody _rigitbody;

    private void Awake()
    {
        _rigitbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        _rigitbody.velocity = new(_joystick.Horizontal * _speed, _rigitbody.velocity.y, _joystick.Vertical * _speed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigitbody.velocity);
            _animator.SetBool("isRunning", true);
        }
        else
            _animator.SetBool("isRunning", false);

        if (_inventory._items.Count > 0)
            _animator.SetBool("IsBuzy", true);
        else
            _animator.SetBool("IsBuzy", false);
    }
}
