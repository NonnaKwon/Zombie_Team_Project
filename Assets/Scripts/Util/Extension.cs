using UnityEngine;

public static class Extension
{
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    public static float GetAngle(Vector3 from, Vector3 to)
    {
        float Dot = Vector3.Dot(to, from);
        return to.x > 0 ? (Mathf.Acos(Dot) * Mathf.Rad2Deg) : -(Mathf.Acos(Dot) * Mathf.Rad2Deg);
    }
}
