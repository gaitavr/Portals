using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private Portal _portal1;
    private Portal _portal2;

    private Rigidbody _rigidbody;
    private Collider _collider;
    private MovementController _movementController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _movementController = GetComponent<MovementController>();
    }

    public void EnterPortal(Portal inPortal, Portal outPortal, Collider[] wallColliders)
    {
        _portal1 = inPortal;
        _portal2 = outPortal;
        for (int i = 0; i < wallColliders.Length; i++)
        {
             Physics.IgnoreCollision(_collider, wallColliders[i]);
        }
    }

    public void Teleport()
    {
        var p1 = _portal1.transform;
        var p2 = _portal2.transform;

        transform.MirrorPosition(p1, p2);
        transform.MirrorRotation(p1, p2);
        _rigidbody.MirrorVelocity(p1, p2);
    }

    public void ExitPortal(Collider[] wallColliders)
    {
        for (int i = 0; i < wallColliders.Length; i++)
        {
            Physics.IgnoreCollision(_collider, wallColliders[i], false);
        }
    }
}
