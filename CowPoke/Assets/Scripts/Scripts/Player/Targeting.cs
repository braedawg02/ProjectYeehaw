using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TargetingSolver
{
    public static List<Vector3> GetDirections(Transform owner, WeaponDef def)
    {
        switch (def.targeting)
        {
            case TargetingMode.ForwardVector: return new() { owner.forward };
            case TargetingMode.Random:        return new() { Random.insideUnitSphere.normalized };
            case TargetingMode.OrbitSelf:     return Orbit(owner);
            case TargetingMode.AOE:           return new() { Vector3.forward }; // direction unused; projectile may be an AOE field
            case TargetingMode.Nearest:
            default:                          return TowardNearest(owner, def.range);
        }
    }

    static List<Vector3> TowardNearest(Transform owner, float range)
    {
        var enemy = EnemyRegistry.GetNearest(owner.position, range); // keep a global/lightweight registry
        Vector3 dir = enemy ? (enemy.position - owner.position).normalized : owner.forward;
        return new() { dir };
    }

    static List<Vector3> Orbit(Transform owner)
    {
        // Example: spawn 4 evenly spaced directions
        var list = new List<Vector3>();
        for (int i = 0; i < 4; i++)
        {
            float angle = i * (360f / 4);
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            list.Add(dir);
        }
        return list;
    }
}
