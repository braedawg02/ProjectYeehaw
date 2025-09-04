using UnityEngine;

public interface IDamageable
{
    void ApplyHit(HitContext ctx);
    Vector3 GetPosition();
}

[System.Serializable]
public struct HitContext
{
    public float damage;
    public bool isCrit;
    public StatusEffectDef[] effects;
    public Vector3 hitPoint;
}

public abstract class StatusEffectDef : ScriptableObject
{
    public string effectId;
    public float duration;

    // Each specific effect (Burn, Freeze, Poison, etc.) will override this
    public abstract void Apply(IDamageable target, float potency);
}
