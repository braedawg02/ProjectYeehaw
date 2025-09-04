using UnityEngine;

public enum FireMode { Auto, Burst, Interval }
public enum TargetingMode { Nearest, Random, ForwardVector, OrbitSelf, AOE }

[CreateAssetMenu(menuName = "Weapons/WeaponDef")]
public class WeaponDef : ScriptableObject
{
    [Header("Meta")]
    public string weaponId;
    public Sprite icon;
    public GameObject WeaponModelPrefab;
    public GameObject projectilePrefab;

    [Header("Core Stats")]
    public float baseDamage = 10f;
    public float shotsPerSecond = 2f;     // fire rate
    public int projectilesPerShot = 1;
    public float spreadDegrees = 0f;
    public float projectileSpeed = 12f;
    public float lifetime = 3f;           // projectile lifetime
    public int pierce = 0;                 // 0 = destroy on first hit

    [Header("Targeting")] // does it orbit, aim at nearest, etc.
    public TargetingMode targeting = TargetingMode.Nearest;
    public float range = 12f;
    public FireMode fireMode = FireMode.Interval;

    [Header("On-Hit")]
    public StatusEffectDef[] onHitEffects; // bleed, burn, slow, etc.
    public bool critEnabled = true;
    public float critChance = 0.1f;       // 10%
    public float critMultiplier = 2.0f;

    [Header("Scaling Curves (per level)")]
    public AnimationCurve damageByLevel = AnimationCurve.Linear(1, 1, 8, 2); // x=level, y=mult
    public AnimationCurve aspdByLevel = AnimationCurve.Linear(1, 1, 8, 1.6f);
}


