using UnityEngine;

public static class PortalExtensions
{
    private static readonly Quaternion _halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    public static void MirrorPosition(this Transform target, Transform p1, Transform p2)
    {
        Vector3 relativePos = p1.InverseTransformPoint(target.position);
        relativePos = _halfTurn * relativePos;
        target.position = p2.TransformPoint(relativePos);
    }

    public static void MirrorRotation(this Transform target, Transform p1, Transform p2)
    {
        Quaternion relativeRot = Quaternion.Inverse(p1.rotation) * target.rotation;
        relativeRot = _halfTurn * relativeRot;
        target.rotation = p2.rotation * relativeRot;
    }
}
