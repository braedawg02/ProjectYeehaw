public class WeaponController
{
    readonly Transform _owner;
    readonly WeaponDef _def;
    readonly ObjectPool _pool;
    int _level = 1;
    float _acc;  // time accumulator

    public WeaponController(Transform owner, WeaponDef def, ObjectPool pool)
    { _owner = owner; _def = def; _pool = pool; }

    public void SetLevel(int level) => _level = Mathf.Max(1, level);

    public void Tick(float dt)
    {
        // Compute current interval from curves
        float sps = Mathf.Max(0.01f, _def.shotsPerSecond * _def.aspdByLevel.Evaluate(_level));
        float interval = 1f / sps;

        _acc += dt;
        // Fire as many times as needed if we fell behind
        while (_acc >= interval)
        {
            _acc -= interval;
            FireVolley();
        }
    }

    void FireVolley()
    {
        var dirs = TargetingSolver.GetDirections(_owner, _def);
        var payload = BuildHitPayload();

        foreach (var dir in dirs)
        {
            for (int i = 0; i < _def.projectilesPerShot; i++)
            {
                float spread = (_def.spreadDegrees == 0) ? 0f : Random.Range(-_def.spreadDegrees, _def.spreadDegrees);
                Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, spread, 0);

                var go = _pool.Spawn(_def.projectilePrefab, _owner.position, rot);
                var proj = go.GetComponent<Projectile>();
                proj.Init(payload, _def.projectileSpeed, _def.lifetime, _def.pierce);
            }
        }
        // Optional: SFX/VFX here
    }

    HitPayload BuildHitPayload()
    {
        float dmg = _def.baseDamage * _def.damageByLevel.Evaluate(_level);
        bool crit = _def.critEnabled && Random.value < _def.critChance;
        if (crit) dmg *= _def.critMultiplier;

        return new HitPayload {
            damage = dmg,
            isCrit = crit,
            effects = _def.onHitEffects
        };
    }
}
