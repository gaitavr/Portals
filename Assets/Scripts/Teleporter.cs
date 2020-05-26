using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private Portal _portal1;
    private Portal _portal2;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void EnterPortal(Portal enterPortal, Portal exitPortal, Collider[] wallColliders)
    {
        _portal1 = enterPortal;
        _portal2 = exitPortal;
        for (int i = 0; i < wallColliders.Length; i++)
        {
            Physics.IgnoreCollision(_collider, wallColliders[i]);
        }
    }

    public void ExitPortal(Collider[] wallColliders)
    {
        for (int i = 0; i < wallColliders.Length; i++)
        {
            Physics.IgnoreCollision(_collider, wallColliders[i], false);
        }
    }

    public void Teleport()
    {
        var p1 = _portal1.transform;
        var p2 = _portal2.transform;

        transform.MirrorPosition(p1, p2);
        transform.MirrorRotation(p1, p2);
    }
}
