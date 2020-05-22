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

    private static readonly Quaternion _halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

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

    public void Warp()
    {
        var p1Transform = _portal1.transform;
        var p2Transform = _portal2.transform;

        // Update position of object.
        Vector3 relativePos = p1Transform.InverseTransformPoint(transform.position);
        relativePos = _halfTurn * relativePos;
        transform.position = p2Transform.TransformPoint(relativePos);

        // Update rotation of object.
        Quaternion relativeRot = Quaternion.Inverse(p1Transform.rotation) * transform.rotation;
        relativeRot = _halfTurn * relativeRot;
        transform.rotation = p2Transform.rotation * relativeRot;

        // Update velocity of _rigidbody.
        Vector3 relativeVel = p1Transform.InverseTransformDirection(_rigidbody.velocity);
        relativeVel = _halfTurn * relativeVel;
        _rigidbody.velocity = p2Transform.TransformDirection(relativeVel);

        // Swap portal references.
        //var tmp = _portal1;
        //_portal1 = _portal2;
        //_portal2 = tmp;
        _movementController.ResetTargetRotation();
    }

    public void ExitPortal(Collider[] wallColliders)
    {
        for (int i = 0; i < wallColliders.Length; i++)
        {
            Physics.IgnoreCollision(_collider, wallColliders[i], false);
        }
    }
}
